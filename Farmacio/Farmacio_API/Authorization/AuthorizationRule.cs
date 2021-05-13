using Microsoft.AspNetCore.Http;

namespace Farmacio_API.Authorization
{
    public abstract class AuthorizationRule : IAuthorizationRule
    {
        public HttpContext HttpContext { get; set; }

        public abstract bool IsAuthorized();
    }
}