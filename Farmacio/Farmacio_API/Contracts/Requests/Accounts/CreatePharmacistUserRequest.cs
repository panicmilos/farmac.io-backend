using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePharmacistUserRequest : CreateUserRequest
    {
        public Guid PharmacyId { get; set; }
    }
}