using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePharmacistUserRequest : UpdateUserRequest
    {
        public Guid PharmacyId { get; set; }
    }
}