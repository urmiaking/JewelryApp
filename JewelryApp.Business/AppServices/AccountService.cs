using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JewelryApp.Data.Models.Identity;
using JewelryApp.Business.Interfaces;
using ErrorOr;
using JewelryApp.Common.Errors;
using Microsoft.Extensions.Logging;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Responses.Authentication;
using JewelryApp.Common.Settings;

namespace JewelryApp.Business.AppServices;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signinManager;

    public AccountService(
        ILogger<AccountService> logger,
        IOptions<JwtSettings> jwtSettingsOption,
        IRefreshTokenService refreshTokenService,
        TokenValidationParameters tokenValidationParameters,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signinManager)
    {
        _logger = logger;
        _refreshTokenService = refreshTokenService;
        _tokenValidationParameters = tokenValidationParameters;
        _jwtSettings = jwtSettingsOption.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _signinManager = signinManager;
    }

    public async Task<ErrorOr<AuthenticationResponse?>> AuthenticateAsync(AuthenticationRequest request)
    {
        try
        {
            var signinResult = await _signinManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (signinResult.Succeeded)
                return await GenerateTokenForUserAsync(request.UserName);
            
            return Errors.Authentication.InvalidCredentials;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Errors.General.ServerError;
        }
        
    }

    public async Task<ErrorOr<AuthenticationResponse?>> RefreshAsync(RefreshTokenRequest request)
    {
        try
        {
            var validatedToken = GetPrincipalFromToken(request.Token);

            if (validatedToken == null)
                return Errors.Authentication.InvalidToken;

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix)
                .Subtract(_jwtSettings.TokenLifeTime);

            if (expiryDateUtc > DateTime.UtcNow)
                return Errors.Authentication.TokenNotExpired;

            var jti = Guid.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
            var userId = Guid.Parse(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var userName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;

            // get stored token
            var refreshToken = await _refreshTokenService.FindAsync(request.RefreshToken);
            if (refreshToken == null)
                return Errors.Authentication.RefreshTokenNotFound;

            if (refreshToken.ExpiryDate < DateTime.UtcNow)
                return Errors.Authentication.TokenExpired;

            if (refreshToken.Invalidated)
                return Errors.Authentication.RefreshTokenInvalidated;

            if (refreshToken.Used)
            {
                // set it as invalidated
                await _refreshTokenService.SetInvalidatedAsync(refreshToken.Id);

                // TODO:
                // We would need a middleware to un-authorize requests with a valid, but invalidated token.

                return Errors.Authentication.RefreshTokenUsed;
            }

            if (refreshToken.JwtId != jti || refreshToken.UserId != userId)
                return Errors.Authentication.RefreshTokenNotValid;

            // set it as used
            await _refreshTokenService.SetUsedAsync(refreshToken.Id);

            return await GenerateTokenForUserAsync(userName);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Errors.General.ServerError;
        }
        
    }

    public async Task<ErrorOr<ChangePasswordResponse?>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user is null)
                return Errors.User.NotFound;
            
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            
            if (result.Succeeded)
                return new ChangePasswordResponse("تغییر رمز با موفقیت انجام شد", true);

            return Errors.Authentication.PasswordNotValid;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Errors.General.ServerError;
        }
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
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                return null;

            return principal;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
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
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(user.UserName))
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

        foreach (var userRole in user.UserRoles)
        {
            if (!string.IsNullOrEmpty(userRole.Role.Name))
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
        }

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