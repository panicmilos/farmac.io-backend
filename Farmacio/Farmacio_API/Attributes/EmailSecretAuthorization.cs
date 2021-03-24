using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Farmacio_API.Attributes
{
    public class EmailSecretAuthorization : Attribute, IAsyncActionFilter
    {
        private readonly string _emailSecret;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParamethers;

        public EmailSecretAuthorization()
        {
            _emailSecret = Environment.GetEnvironmentVariable("EmailSecret");
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationParamethers = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_emailSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var authorization = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
                var token = authorization.Substring(7);

                _tokenHandler.ValidateToken(token, _tokenValidationParamethers, out _);

                var accountId = GetClaim(token, "name");
                context.HttpContext.Request.Headers.Add("accountId", accountId);

                await next();
            }
            catch (SecurityTokenExpiredException)
            {
                throw new BadLogicException("Email verification link has expired.");
            }
            catch (Exception)
            {
                throw new BadLogicException("Email verification link is not valid.");
            }
        }

        private string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}