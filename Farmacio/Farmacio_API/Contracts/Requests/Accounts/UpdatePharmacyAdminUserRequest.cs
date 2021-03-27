using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePharmacyAdminUserRequest : UpdateUserRequest
    {
        public Guid PharmacyId { get; set; }
    }
}