using System;

namespace Farmacio_Models.DTO
{
    public class MakeAppointmentWithDermatologistDTO
    {
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
