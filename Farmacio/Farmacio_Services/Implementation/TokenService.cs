using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _emailSecret;

        public TokenService()
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JwtSecret");
            _emailSecret = Environment.GetEnvironmentVariable("EmailSecret");
        }

        public string GenerateAuthTokenFor(Account account)
        {
            return GenerateTokenFor(account, _jwtSecret, 30);
        }

        public string GenerateEmailTokenFor(Account account)
        {
            return GenerateTokenFor(account, _emailSecret, 24 * 60);
        }

        private string GenerateTokenFor(Account account, string secret, int expiresFor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.Id.ToString()),
                    new Claim(ClaimTypes.Role, account.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresFor),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}