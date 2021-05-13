using Microsoft.AspNetCore.Http;

namespace Farmacio_API.Authorization
{
    public interface IAuthorizationRule
    {
        HttpContext HttpContext { get; set; }

        bool IsAuthorized();
    }
}