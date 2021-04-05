using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistService : IMedicalStaffService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);

        Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId);

        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);

        Account AddToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTime workTime);

        Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistId);
    }
}