using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacistService : AccountService, IPharmacistService
    {
        private readonly ICrudService<Pharmacy> _pharmacyService;
        
        public PharmacistService(IEmailVerificationService emailVerificationService, ICrudService<Pharmacy> pharmacyService,
            IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
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

        public override Account Create(Account account)
        {
            ValidatePharmacyId(account);
            return base.Create(account);
        }

        public override Account Update(Account account)
        {
            ValidatePharmacyId(account);

            var pharmacist = Read(account.Id);
            pharmacist.User.FirstName = account.User.FirstName;
            pharmacist.User.LastName = account.User.LastName;
            pharmacist.User.PhoneNumber = account.User.PhoneNumber;
            pharmacist.User.PID = account.User.PID;
            pharmacist.User.DateOfBirth = account.User.DateOfBirth;

            pharmacist.User.Address.State = account.User.Address.State;
            pharmacist.User.Address.City = account.User.Address.City;
            pharmacist.User.Address.StreetName = account.User.Address.StreetName;
            pharmacist.User.Address.StreetNumber = account.User.Address.StreetNumber;
            pharmacist.User.Address.Lat = account.User.Address.Lat;
            pharmacist.User.Address.Lng = account.User.Address.Lng;
            
            return base.Update(pharmacist);
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
            if (_pharmacyService.Read(((Pharmacist) account.User).PharmacyId) == null)
                throw new MissingEntityException("Pharmacy with the provided id does not exist.");
        }

        private static IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(p => ((Pharmacist) p.User).PharmacyId == pharmacyId);
        }
    }
}