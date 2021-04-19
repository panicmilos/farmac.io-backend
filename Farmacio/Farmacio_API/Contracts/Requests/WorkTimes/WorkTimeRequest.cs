using System;

namespace Farmacio_API.Contracts.Requests.WorkTimes
{
    public class WorkTimeRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}