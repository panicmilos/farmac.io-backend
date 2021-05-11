using Microsoft.AspNetCore.Http;
using System;

namespace Farmacio_API.Authorization
{
    public class AccountSpecific : AuthorizationRule
    {
        public readonly Guid _specificId;

        public AccountSpecific(Guid specificId)
        {
            _specificId = specificId;
        }

        public static IAuthorizationRule For(Guid specificId)
        {
            return new AccountSpecific(specificId);
        }

        public override bool IsAuthorized()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizedAccountId = new Guid(HttpContext.User.Identity.Name);
                return authorizedAccountId == _specificId;
            }
            return false;
        }
    }
}