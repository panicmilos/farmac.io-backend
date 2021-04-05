using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class MakeAppointmentWithDermatologistDTO
    {
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
