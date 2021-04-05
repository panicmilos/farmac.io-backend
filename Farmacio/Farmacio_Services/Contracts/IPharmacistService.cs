using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacistService : IMedicalStaffService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);
        Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId);
        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);
    }
}