using System;

namespace Farmacio_API.Contracts.Requests.PharmacyReports
{
    public class TimePeriodRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}