using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SystemAdminService : AccountService, ISystemAdminService
    {
        public SystemAdminService(IEmailVerificationService emailVerificationService, IAccountRepository repository) :
            base(emailVerificationService, repository)
        {
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.SystemAdmin).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.SystemAdmin ? account : null;
        }

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("System Admin account not found.");
            return existingAccount;
        }
    }
}