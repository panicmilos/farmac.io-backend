using System;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Farmacio_Repositories.Implementation
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DatabaseContext context) :
            base(context)
        {
        }

        public IEnumerable<Appointment> ReadForPatient(Guid patientId)
        {
            return _context.Appointments
                .FromSqlRaw($"SELECT * FROM Appointments WHERE PatientId = \"{patientId}\" FOR UPDATE;").ToList();
        }

        public IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId)
        {
            return _context.Appointments
                .FromSqlRaw($"SELECT * FROM Appointments WHERE MedicalStaffId = \"{medicalStaffId}\" FOR UPDATE;")
                .ToList();
        }
    }
}