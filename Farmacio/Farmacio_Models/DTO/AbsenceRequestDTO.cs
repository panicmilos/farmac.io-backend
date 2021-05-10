using Farmacio_Models.Domain;
using System;

namespace Farmacio_Models.DTO
{
    public class AbsenceRequestDTO
    {
        public Guid RequesterId { get; set; }
        public AbsenceType Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
