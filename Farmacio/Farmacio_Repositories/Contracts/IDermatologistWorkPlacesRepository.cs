using System;
using Farmacio_Models.Domain;
using System.Collections.Generic;

namespace Farmacio_Repositories.Contracts
{
    public interface IDermatologistWorkPlacesRepository : IRepository<DermatologistWorkPlace>
    {
        IEnumerable<DermatologistWorkPlace> ReadForDermatologist(Guid dermatologistId);
        DermatologistWorkPlace ReadForDermatologistInPharmacy(Guid dermatologistId, Guid pharmacyId);
    }
}