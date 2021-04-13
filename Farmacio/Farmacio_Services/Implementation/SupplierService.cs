using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierService : AccountService, ISupplierService
    {
        public SupplierService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Supplier).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Supplier ? account : null;
        }

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("Supplier account not found.");
            return existingAccount;
        }
    }
}