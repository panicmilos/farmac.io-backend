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

        public IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalAccountId)
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

        public IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalAccountId, string crit, bool isAsc)
        {
            var sortingCriteria = new Dictionary<string, Func<PatientDTO, object>>()
            {
                { "firstName", p => p.FirstName },
                { "lastName", p => p.LastName },
                { "appointmentDate", p => p.AppointmentDate }
            };
            var patients = ReadPatientsForMedicalStaff(medicalAccountId);
            if (sortingCriteria.TryGetValue(crit ?? "", out var sortCrit))
                patients = isAsc ? patients.OrderBy(sortCrit) : patients.OrderByDescending(sortCrit);
            return patients;
        }

        public IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalAccountId, string name)
        {
            return ReadPatientsForMedicalStaff(medicalAccountId).Where(p =>
                name == null ||
                $"{p.FirstName.ToLower()} {p.LastName.ToLower()}".Contains(name.ToLower()));
        }
    }
}
