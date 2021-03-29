using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePharmacyAdminUserRequest : CreateUserRequest
    {
        public Guid PharmacyId { get; set; }
    }
}