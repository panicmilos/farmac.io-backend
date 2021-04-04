using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacistService : AccountService, IPharmacistService
    {
        private readonly IPharmacyService _pharmacyService;

        public PharmacistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService,
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
    }
}