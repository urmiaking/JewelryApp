using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Settings;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Responses.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Errors = JewelryApp.Shared.Errors.Errors;

namespace JewelryApp.Application.AppServices;

[ScopedService<IAccountService>]
public class AccountService : IAccountService
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signinManager;

    public AccountService(
        IOptions<JwtSettings> jwtSettingsOption,
        IRefreshTokenService refreshTokenService,
        TokenValidationParameters tokenValidationParameters,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signinManager)
    {
        _refreshTokenService = refreshTokenService;
        _tokenValidationParameters = tokenValidationParameters;
        _jwtSettings = jwtSettingsOption.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _signinManager = signinManager;
    }

    public async Task<ErrorOr<AuthenticationResponse?>> AuthenticateAsync(AuthenticationRequest request, CancellationToken token = default)
    {
        var signinResult = await _signinManager.PasswordSignInAsync(request.UserName, request.Password, true, true);

        if (signinResult.Succeeded)
            return await GenerateTokenForUserAsync(request.UserName);

        return Errors.Authentication.InvalidCredentials;
    }

    public async Task<ErrorOr<AuthenticationResponse?>> RefreshAsync(RefreshTokenRequest request, CancellationToken token = default)
    {
        var validatedToken = GetPrincipalFromToken(request.Token);

        if (validatedToken == null)
            return Errors.Authentication.InvalidToken;

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix)
            .Subtract(_jwtSettings.TokenLifeTime);

        if (expiryDateUtc > DateTime.UtcNow)
            throw new Exception("token has not expired yet");

        var jti = Guid.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
        var userId = Guid.Parse(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var userName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;

        // get stored token
        var refreshToken = await _refreshTokenService.FindAsync(request.RefreshToken);
        if (refreshToken == null)
            throw new Exception("refresh token not found");

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
            throw new Exception("refresh token expired");

        if (refreshToken.Invalidated)
            throw new Exception("refresh token invalidated");

        if (refreshToken.Used)
        {
            // set it as invalidated
            await _refreshTokenService.SetInvalidatedAsync(refreshToken.Id);

            // TODO:
            // We would need a middleware to un-authorize requests with a valid, but invalidated token.

            throw new Exception("refresh token used");
        }

        if (refreshToken.JwtId != jti || refreshToken.UserId != userId)
            throw new Exception("refresh token is not valid");

        // set it as used
        await _refreshTokenService.SetUsedAsync(refreshToken.Id);

        return await GenerateTokenForUserAsync(userName);
    }

    public async Task<ErrorOr<ChangePasswordResponse?>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken token = default)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null)
            return Errors.User.NotFound;

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (result.Succeeded)
            return new ChangePasswordResponse("تغییر رمز با موفقیت انجام شد", true);

        return Errors.Authentication.PasswordNotValid;
    }

    private async Task<AuthenticationResponse?> GenerateTokenForUserAsync(string userName)
    {
        var user = await _userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(x => x.UserName == userName);

        if (user != null)
        {
            return await GenerateTokenForUserAsync(user);
        }

        return null;
    }

    private async Task<AuthenticationResponse?> GenerateTokenForUserAsync(AppUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetUserClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        var jti = Guid.Parse(claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

        var refreshToken =
            await _refreshTokenService.AddAsync(user.Id, _jwtSettings.RefreshTokenLifeTime, jti);

        return new AuthenticationResponse(token, refreshToken);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            _tokenValidationParameters.ValidateLifetime = false;
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(AppUser user)
    {
        var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (securityStampClaimType, user.SecurityStamp ?? string.Empty)
        };

        if (!string.IsNullOrEmpty(user.UserName))
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

        claims.AddRange(from userRole in user.UserRoles where !string.IsNullOrEmpty(userRole.Role.Name) select new Claim(ClaimTypes.Role, userRole.Role.Name));

        // user claims
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        // user-roles claims
        foreach (var userRole in user.UserRoles)
        {
            if (string.IsNullOrEmpty(userRole.Role.Name))
                continue;

            var role = await _roleManager.FindByNameAsync(userRole.Role.Name);
            if (role == null)
                continue;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims);
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}