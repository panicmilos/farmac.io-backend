using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;

namespace Farmacio_API.Contracts.Requests.PharmacyReports
{
    public class TimePeriodRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}