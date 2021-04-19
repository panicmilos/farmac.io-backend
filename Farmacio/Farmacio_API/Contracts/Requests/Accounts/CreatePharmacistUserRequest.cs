using System;
using Farmacio_API.Contracts.Requests.WorkTimes;
using Farmacio_Models.Domain;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePharmacistUserRequest : CreateUserRequest
    {
        public Guid PharmacyId { get; set; }
        public WorkTimeRequest WorkTime { get; set; }
    }
}