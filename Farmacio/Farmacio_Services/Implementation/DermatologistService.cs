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
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId)
        {
            var account = Read(dermatologistId);
            var dermatologist = (Dermatologist) account?.User;
            return dermatologist?.WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId) != null
                ? account
                : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        private static IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(d =>
                ((Dermatologist) d.User).WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId) != null);
        }
    }
}