using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Requests.AbsenceRequests
{
    public class DeclineAbsenceRequestRequest
    {
        public string Reason { get; set; }
    }
}
