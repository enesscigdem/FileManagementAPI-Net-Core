using FileOrbis.EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileOrbisApi.JWT
{
    public class CreateToken
    {
        public string GenerateJwtToken(UserInfo user, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                claims: new[] { new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()) },
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddYears(2),
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
