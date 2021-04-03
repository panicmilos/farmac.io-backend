using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacistService : IAccountService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);
        Account ReadForPharmacy(Guid pharmacyId, Guid pharmacistId);
        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);
        IEnumerable<PatientDTO> GetPatients(Guid pharmacistId);
    }
}