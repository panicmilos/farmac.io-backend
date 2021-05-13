using Farmacio_Models.Domain;
using System;
using System.Linq;

namespace Farmacio_API.Authorization
{
    public class AllDataAllowed : AuthorizationRule
    {
        private readonly Role _role;

        public AllDataAllowed(Role role)
        {
            _role = role;
        }

        public static IAuthorizationRule For(Role role)
        {
            return new AllDataAllowed(role);
        }

        public override bool IsAuthorized()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Enum.TryParse(HttpContext.User.Claims.First(claim => claim.Type.EndsWith("/role")).Value, out Role authorizedRole);
                return authorizedRole == _role;
            }
            return false;
        }
    }
}