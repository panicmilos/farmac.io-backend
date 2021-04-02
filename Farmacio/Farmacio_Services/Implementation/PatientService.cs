using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PatientService : AccountService, IPatientService
    {
        public PatientService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
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
        
        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if(existingAccount == null || existingAccount.Role != Role.Patient)
                throw new MissingEntityException("Patient account not found.");
            return existingAccount;
        }
    }
}