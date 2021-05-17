using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using System;

namespace Farmacio_API.Authorization
{
    public class IsPharmacyAdmin : AuthorizationRule
    {
        private readonly Guid _pharmacyId;

        public IsPharmacyAdmin(Guid pharmacyId)
        {
            _pharmacyId = pharmacyId;
        }

        public static IAuthorizationRule Of(Guid pharmacyId)
        {
            return new IsPharmacyAdmin(pharmacyId);
        }

        public override bool IsAuthorized()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var pharmacyAdminService = GetService<IPharmacyAdminService>();
            var pharmacyAdmin = pharmacyAdminService.Read(Guid.Parse(HttpContext.User.Identity.Name));

            if (pharmacyAdmin == null)
            {
                return false;
            }

            return (pharmacyAdmin.User as PharmacyAdmin).PharmacyId == _pharmacyId;
        }
    }
}