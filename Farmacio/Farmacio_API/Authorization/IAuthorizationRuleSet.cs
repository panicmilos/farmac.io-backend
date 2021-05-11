using Microsoft.AspNetCore.Http;

namespace Farmacio_API.Authorization
{
    public interface IAuthorizationRuleSet
    {
        HttpContext HttpContext { get; set; }

        IAuthorizationRuleSet Rule(IAuthorizationRule rule);

        IAuthorizationRuleSet And(IAuthorizationRule rule);

        IAuthorizationRuleSet Or(IAuthorizationRule rule);

        void Authorize();
    }
}