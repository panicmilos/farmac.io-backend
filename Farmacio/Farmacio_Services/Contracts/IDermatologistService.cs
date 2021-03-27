using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistService : IAccountService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);
        Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId);
        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);
    }
}