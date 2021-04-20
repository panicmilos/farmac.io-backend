using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Responses.Dermatologists
{
    public class PatientFollowResponse
    {
        public Guid FollowId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}