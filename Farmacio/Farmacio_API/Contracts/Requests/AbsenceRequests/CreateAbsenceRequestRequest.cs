using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Requests.AbsenceRequests
{
    public class CreateAbsenceRequestRequest
    {
        public Guid RequesterId { get; set; }
        public AbsenceType Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
