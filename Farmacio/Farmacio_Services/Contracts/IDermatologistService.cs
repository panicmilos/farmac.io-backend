using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistService : IMedicalStaffService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);

        Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistAccountId);

        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);

        Account AddToPharmacy(Guid pharmacyId, Guid dermatologistAccountId, WorkTime workTime);

        Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistAccountId);
    }
}