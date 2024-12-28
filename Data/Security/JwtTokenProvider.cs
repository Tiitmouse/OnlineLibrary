using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Data.Security;

public class JwtTokenProvider
{
    public static string CreateToken(string secureKey, int expiration, string subject = null, string role = null)
    {
        // Get secret key bytes
        var tokenKey = Encoding.UTF8.GetBytes(secureKey);

        // Create a token descriptor (represents a token, kind of a "template" for token)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(expiration),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        List<Claim> claims = new();

        if (!string.IsNullOrEmpty(subject))
        {
            claims.Add(new Claim(ClaimTypes.Name, subject));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
        }

        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        tokenDescriptor.Subject = new ClaimsIdentity(claims);

        // Create token using that descriptor, serialize it and return it
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var serializedToken = tokenHandler.WriteToken(token);

        return serializedToken;
    }
}
