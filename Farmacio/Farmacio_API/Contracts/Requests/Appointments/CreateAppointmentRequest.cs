using System;

namespace Farmacio_API.Contracts.Requests.Appointments
{
    public class CreateAppointmentRequest
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public float? Price { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid MedicalStaffId { get; set; }
        public Guid? PatientId { get; set; }
    }
}