using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacistService : IAccountService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);
        Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId);
    }
}