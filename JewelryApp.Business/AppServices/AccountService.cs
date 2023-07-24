using AutoMapper;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JewelryApp.Data.Models;
using JewelryApp.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signinManager;

    public AccountService(IMapper mapper,
        IOptions<JwtSettings> jwtSettingsOption,
        IRefreshTokenService refreshTokenService,
        TokenValidationParameters tokenValidationParameters,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signinManager)
    {
        _mapper = mapper;
        _refreshTokenService = refreshTokenService;
        _tokenValidationParameters = tokenValidationParameters;
        _jwtSettings = jwtSettingsOption.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _signinManager = signinManager;
    }

    public async Task<UserTokenDto?> AuthenticateAsync(LoginDto request)
    {
        var signinResult = await _signinManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

        if (signinResult.Succeeded)
        {
            return await GenerateTokenForUserAsync(request.UserName);
        }

        return null;
    }

    public async Task<UserTokenDto?> RefreshAsync(UserTokenDto request)
    {
        var validatedToken = GetPrincipalFromToken(request.Token);

        if (validatedToken == null)
            throw new ValidationException("Invalid token");

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix)
            .Subtract(_jwtSettings.TokenLifeTime);

        if (expiryDateUtc > DateTime.UtcNow)
            throw new ValidationException("The token has not expired yet.");

        var jti = Guid.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
        var userId = Guid.Parse(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var userName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;

        // get stored token
        var refreshToken = await _refreshTokenService.FindAsync(request.RefreshToken);
        if (refreshToken == null)
            throw new ValidationException("Refresh token not found.");

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
            throw new ValidationException("Refresh token is expired.");

        if (refreshToken.Invalidated)
            throw new ValidationException("Refresh token has been invalidated.");

        if (refreshToken.Used)
        {
            // set it as invalidated
            await _refreshTokenService.SetInvalidatedAsync(refreshToken.Id);

            // TODO:
            // We would need a middleware to un-authorize requests with a valid, but invalidated token.

            throw new ValidationException("Refresh token has been used.");
        }

        if (refreshToken.JwtId != jti || refreshToken.UserId != userId)
            throw new ValidationException("Refresh token is not valid.");

        // set it as used
        await _refreshTokenService.SetUsedAsync(refreshToken.Id);

        return await GenerateTokenForUserAsync(userName);
    }

    private async Task<UserTokenDto?> GenerateTokenForUserAsync(string userName)
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

    private async Task<UserTokenDto?> GenerateTokenForUserAsync(ApplicationUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetUserClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        var jti = Guid.Parse(claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

        var refreshToken =
            await _refreshTokenService.AddAsync(user.Id, _jwtSettings.RefreshTokenLifeTime, jti);

        return new UserTokenDto(token, refreshToken);
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

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(ApplicationUser user)
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