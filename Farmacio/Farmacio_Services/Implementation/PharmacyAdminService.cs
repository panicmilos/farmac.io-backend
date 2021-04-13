using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PharmacyAdminService : AccountService, IPharmacyAdminService
    {
        private readonly IPharmacyService _pharmacyService;

        public PharmacyAdminService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService,
            IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.PharmacyAdmin).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.PharmacyAdmin ? account : null;
        }
        
        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if(existingAccount == null)
                throw new MissingEntityException("Pharmacy Admin account not found.");
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
            ((PharmacyAdmin) existingAccount.User).PharmacyId = ((PharmacyAdmin) account.User).PharmacyId;
            
            return _repository.Update(existingAccount);
        }

        private void ValidatePharmacyId(Account account)
        {
            _pharmacyService.TryToRead(((PharmacyAdmin) account.User).PharmacyId);
        }
    }
}