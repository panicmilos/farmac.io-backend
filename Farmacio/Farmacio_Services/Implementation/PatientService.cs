using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PatientService : AccountService, IPatientService
    {
        public PatientService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
        }

        public override Account Create(Account account)
        {
            return base.Create(account);
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

        public override Account Update(Account account)
        {
            return base.Update(account);
        }

        public override Account Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}