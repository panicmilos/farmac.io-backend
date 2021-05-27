using Farmacio_API.Contracts.Requests.WorkTimes;
using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePharmacistUserRequest : UpdateUserRequest
    {
        public Guid PharmacyId { get; set; }
        public WorkTimeRequest WorkTime { get; set; }
    }
}