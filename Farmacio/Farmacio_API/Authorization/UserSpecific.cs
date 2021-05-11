using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Farmacio_API.Authorization
{
    public class UserSpecific : AuthorizationRule
    {
        public readonly Guid _specificId;

        public UserSpecific(Guid specificId)
        {
            _specificId = specificId;
        }

        public static IAuthorizationRule For(Guid specificId)
        {
            return new UserSpecific(specificId);
        }

        public override bool IsAuthorized()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizedUserId = new Guid(HttpContext.User.Claims.First(claim => claim.Type == "UserId").Value);
                return authorizedUserId == _specificId;
            }
            return false;
        }
    }
}