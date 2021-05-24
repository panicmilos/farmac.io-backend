using System;
using Farmacio_Models.Domain;
using System.Collections.Generic;

namespace Farmacio_Repositories.Contracts
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        IEnumerable<Appointment> ReadForPatient(Guid patientId);
        IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId);
    }
}