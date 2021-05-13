using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_API.Authorization
{
    public class AuthorizationRuleSet : IAuthorizationRuleSet
    {
        public HttpContext HttpContext { get; set; }
        public IList<IList<IAuthorizationRule>> _rules;

        public AuthorizationRuleSet()
        {
            _rules = new List<IList<IAuthorizationRule>>()
            {
                new List<IAuthorizationRule>()
            };
        }

        public AuthorizationRuleSet(HttpContext httpContext) :
            this()
        {
            HttpContext = httpContext;
        }

        public static IAuthorizationRuleSet For(HttpContext httpContext)
        {
            return new AuthorizationRuleSet(httpContext);
        }

        public IAuthorizationRuleSet Rule(IAuthorizationRule rule)
        {
            rule.HttpContext = HttpContext;
            _rules.Last().Add(rule);

            return this;
        }

        public IAuthorizationRuleSet And(IAuthorizationRule rule)
        {
            return Rule(rule);
        }

        public IAuthorizationRuleSet Or(IAuthorizationRule rule)
        {
            _rules.Add(new List<IAuthorizationRule>());

            return Rule(rule);
        }

        public void Authorize()
        {
            foreach (var ruleList in _rules)
            {
                if (ruleList.All(rule => rule.IsAuthorized()))
                {
                    return;
                }
            }

            throw new ForbiddenException();
        }
    }
}