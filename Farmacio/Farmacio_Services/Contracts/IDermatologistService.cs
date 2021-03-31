using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistService : IAccountService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);

        Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId);

        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);

        IEnumerable<PatientDTO> GetPatients(Guid dermatologistId);

        Account AddToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTime workTime);

        Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistId);
    }
}