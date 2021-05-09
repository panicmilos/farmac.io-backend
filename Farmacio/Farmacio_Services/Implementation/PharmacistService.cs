using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.DTO;
using Farmacio_Services.Implementation.Validation;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacistService : MedicalStaffService, IPharmacistService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IAppointmentService _appointmentService;

        public PharmacistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService,
            IAppointmentService appointmentService, IRepository<Account> repository) :
            base(emailVerificationService, appointmentService, repository)
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
            if(existingAccount == null)
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
            WorkTimeValidation.ValidateWorkHours(((Pharmacist) account.User).WorkTime);
            
            var existingAccount = TryToRead(account.Id);

            existingAccount.User.FirstName = account.User.FirstName;
            existingAccount.User.LastName = account.User.LastName;
            existingAccount.User.PhoneNumber = account.User.PhoneNumber;
            existingAccount.User.PID = account.User.PID;
            existingAccount.User.DateOfBirth = account.User.DateOfBirth;

            existingAccount.User.Address.State = account.User.Address.State;
            existingAccount.User.Address.City = account.User.Address.City;
            existingAccount.User.Address.StreetName = account.User.Address.StreetName;
            existingAccount.User.Address.StreetNumber = account.User.Address.StreetNumber;
            existingAccount.User.Address.Lat = account.User.Address.Lat;
            existingAccount.User.Address.Lng = account.User.Address.Lng;
            ((Pharmacist) existingAccount.User).PharmacyId = ((Pharmacist) account.User).PharmacyId;
            ((Pharmacist) existingAccount.User).WorkTime = ((Pharmacist) account.User).WorkTime;
            
            return _repository.Update(existingAccount);
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
        
        public override IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var pharmacyId = filterParams.PharmacyId;
            return pharmacyId != null
                ? FilterByPharmacyId(base.ReadBy(filterParams), new Guid(pharmacyId))
                : base.ReadBy(filterParams);
        }

        private void ValidatePharmacyId(Account account)
        {
            _pharmacyService.TryToRead(((Pharmacist) account.User).PharmacyId);
        }

        private static IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(p => ((Pharmacist) p.User).PharmacyId == pharmacyId);
        }

        public IEnumerable<Pharmacist> SortByGrade(IList<Pharmacist> pharmacists, SearhSortParamsForAppointments searchSortParams)
        {
            if (searchSortParams.SortCriteria != "")
            {
                pharmacists = searchSortParams.IsAsc ? pharmacists.OrderBy(p => p.AverageGrade).ToList() : pharmacists.OrderByDescending(p => p.AverageGrade).ToList();
            }
            return pharmacists;
        }
    }
}