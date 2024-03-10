using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

namespace BankProject;

public class TokenService : ITokenService
{
    public string GenerateToken(UserModel user)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(Settings.SecretJWTToken);
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, user.User.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}
