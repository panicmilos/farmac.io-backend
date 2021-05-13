using Microsoft.AspNetCore.Http;
using System;

namespace Farmacio_API.Authorization
{
    public class AccountSpecific : AuthorizationRule
    {
        public readonly Guid _accountId;

        public AccountSpecific(Guid accountId)
        {
            _accountId = accountId;
        }

        public static IAuthorizationRule For(Guid accountId)
        {
            return new AccountSpecific(accountId);
        }

        public override bool IsAuthorized()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizedAccountId = new Guid(HttpContext.User.Identity.Name);
                return authorizedAccountId == _accountId;
            }
            return false;
        }
    }
}