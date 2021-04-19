using System;

namespace Farmacio_Models.DTO
{
    public class CreateReportDTO
    {
        public Guid AppointmentId { get; set; }
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
    }
}
