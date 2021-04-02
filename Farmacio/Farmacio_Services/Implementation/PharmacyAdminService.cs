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
            if(existingAccount == null || existingAccount.Role != Role.PharmacyAdmin)
                throw new MissingEntityException("Pharmacy Admin account not found.");
            return existingAccount;
        }

        public override Account Create(Account account)
        {
            ValidatePharmacyId(account);
            var createdAccount = base.Create(account);
            SaveChanges();
            return createdAccount;
        }

        public override Account Update(Account account)
        {
            ValidatePharmacyId(account);
            var updatedAccount = base.Update(account);
            SaveChanges();
            return updatedAccount;
        }

        private void ValidatePharmacyId(Account account)
        {
            _pharmacyService.TryToRead(((PharmacyAdmin) account.User).PharmacyId);
        }
    }
}