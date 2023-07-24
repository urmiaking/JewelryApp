using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Client.Security;

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        if (string.IsNullOrEmpty(token))
            return Enumerable.Empty<Claim>();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwt.Claims;
    }

    public static DateTime GetExpireDate(string token)
    {
        var claims = ParseClaimsFromJwt(token);

        return GetExpireDate(claims);
    }

    public static DateTime GetExpireDate(IEnumerable<Claim> claims)
    {
        var exp = claims.FirstOrDefault(x => x.Type == "exp");
        if (exp != null)
            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.Value)).UtcDateTime;

        return DateTime.UtcNow;
    }

    private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
    {
        keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles != null)
        {
            var parsedRoles = roles.ToString()?.Trim().TrimStart('[').TrimEnd(']').Split(',');
            if (parsedRoles != null)
                if (parsedRoles.Length > 1)
                    foreach (var parsedRole in parsedRoles)
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                else
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));

            keyValuePairs.Remove(ClaimTypes.Role);
        }
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        //switch (base64.Length % 4)
        //{
        //    case 2: base64 += "=="; break;
        //    case 3: base64 += "="; break;
        //}
        return Convert.FromBase64String(base64);
    }
}
