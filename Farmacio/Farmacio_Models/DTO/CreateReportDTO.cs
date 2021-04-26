using System;
using System.Collections.Generic;

namespace Farmacio_Models.DTO
{
    public class CreateReportDTO
    {
        public Guid AppointmentId { get; set; }
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
        public List<MedicineQuantityDTO> PrescribedMedicines { get; set; }
    }
}
