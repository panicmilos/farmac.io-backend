using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class DermatologistService : AccountService, IDermatologistService
    {
        public DermatologistService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) : base(emailVerificationService, repository)
        {
        }
        
        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Dermatologist).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Dermatologist ? account : null;
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            throw new NotImplementedException();
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            throw new NotImplementedException();
        }
    }
}