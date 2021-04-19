using System;

namespace Farmacio_Models.DTO
{
    public class CreateAppointmentDTO
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public float? Price { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid MedicalStaffId { get; set; }
        public Guid? PatientId { get; set; }
    }
}