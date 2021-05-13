using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Farmacio_API.Authorization
{
    public class UserSpecific : AuthorizationRule
    {
        public readonly Guid _userId;

        public UserSpecific(Guid userId)
        {
            _userId = userId;
        }

        public static IAuthorizationRule For(Guid userId)
        {
            return new UserSpecific(userId);
        }

        public override bool IsAuthorized()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizedUserId = new Guid(HttpContext.User.Claims.First(claim => claim.Type == "UserId").Value);
                return authorizedUserId == _userId;
            }
            return false;
        }
    }
}