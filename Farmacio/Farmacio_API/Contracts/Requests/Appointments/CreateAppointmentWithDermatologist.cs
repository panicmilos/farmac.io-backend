using System;

namespace Farmacio_API.Contracts.Requests.Appointments
{
    public class CreateAppointmentWithDermatologist
    {
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; }
    }
}