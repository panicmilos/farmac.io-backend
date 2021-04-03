using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;


namespace Farmacio_Services.Implementation
{
    public class MedicalStaffService : AccountService, IMedicalStaffService
    {
        private readonly IAppointmentService _appointmentService;

        public MedicalStaffService(IEmailVerificationService emailVerificationService, IAppointmentService appointmentService,
            IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<PatientDTO> GetPatients(Guid medicalAccountId)
        {
            var medicalAccount = TryToRead(medicalAccountId);

            return _appointmentService
                .ReadForMedicalStaff(medicalAccount.UserId)
                .Where(ap => ap.IsReserved && ap.PatientId != null)
                .Select(ap => new PatientDTO
                {
                    Id = ap.PatientId.Value,
                    FirstName = ap.Patient.FirstName,
                    LastName = ap.Patient.LastName,
                    DateOfBirth = ap.Patient.DateOfBirth,
                    Address = ap.Patient.Address,
                    PhoneNumber = ap.Patient.PhoneNumber,
                    AppointmentDate = ap.DateTime
                });
        }
    }
}
