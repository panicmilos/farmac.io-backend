using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalExceptionHandler.Exceptions;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Implementation
{
    public class PatientService : AccountService, IPatientService
    {
        private readonly IAppointmentService _appointmentService;

        public PatientService(IEmailVerificationService emailVerificationService, IAppointmentService appointmentService,
            IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
        }

        public bool HasExceededLimitOfNegativePoints(Guid patientId)
        {
            var account = base.Read().Where(account => account.UserId == patientId).FirstOrDefault();
            if(account == null)
            {
                throw new BadLogicException("The given patient does not exixst in the system.");
            }
            var patient = (Patient)account.User;
            return patient.NegativePoints >= 3;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Patient).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);
            return account?.Role == Role.Patient ? account : null;
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
                { "firstName", a => a.FirstName },
                { "lastName", a => a.LastName },
                { "appointmentDate", a => a.AppointmentDate }
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

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if(existingAccount == null)
                throw new MissingEntityException("Patient account not found.");
            return existingAccount;
        }
    }
}