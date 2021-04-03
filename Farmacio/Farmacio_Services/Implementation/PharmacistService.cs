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
    public class PharmacistService : AccountService, IPharmacistService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPharmacyService _pharmacyService;

        public PharmacistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService, 
            IAppointmentService appointmentService, IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
            _appointmentService = appointmentService;

        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Pharmacist).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Pharmacist ? account : null;
        }
        
        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if(existingAccount == null || existingAccount.Role != Role.Pharmacist)
                throw new MissingEntityException("Pharmacist account not found.");
            return existingAccount;
        }

        public override Account Create(Account account)
        {
            ValidatePharmacyId(account);
            return base.Create(account);
        }

        public override Account Update(Account account)
        {
            ValidatePharmacyId(account);
            return base.Update(account);
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId)
        {
            var account = Read(pharmacistId);
            var pharmacist = (Pharmacist) account?.User;
            return pharmacist?.PharmacyId == pharmacistId ? account : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        private void ValidatePharmacyId(Account account)
        {
            _pharmacyService.TryToRead(((Pharmacist) account.User).PharmacyId);
        }

        private static IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(p => ((Pharmacist) p.User).PharmacyId == pharmacyId);
        }

        public IEnumerable<PatientDTO> GetPatients(Guid pharmacistId)
        {
            var pharmacistAccount = TryToRead(pharmacistId);

            return _appointmentService
                .ReadForMedicalStaff(pharmacistAccount.UserId)
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