using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;


namespace Farmacio_Services.Implementation
{
    public class DermatologistService : AccountService, IDermatologistService
    {
        public DermatologistService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) : base(emailVerificationService, repository)
        {
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Dermatologist).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Dermatologist ? account : null;
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            throw new NotImplementedException();
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PatientDTO> GetPatients(Guid dermatologistId)
        {
            var account = base.Read(dermatologistId);
            var dermatologist = (Dermatologist)account?.User;
            if (dermatologist == null)
                throw new MissingEntityException();
            List<PatientDTO> patients = new List<PatientDTO>();
            foreach (var appointment in dermatologist?.Appointments)
            {
                var patient = appointment.Patient;
                if (patient == null) continue;
                patients.Add(new PatientDTO
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    DateOfBirth = patient.DateOfBirth,
                    Address = patient.Address,
                    PhoneNumber = patient.PhoneNumber,
                    AppointmentDate = appointment.DateTime
                });
            }
            return patients;
        }

    }
}