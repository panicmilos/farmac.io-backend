using System;

namespace Farmacio_API.Contracts.Requests.Followings
{
    public class CreatePatientFollowRequest
    {
        public Guid PatientId { get; set; }
        public Guid PharmacyId { get; set; }
    }
}