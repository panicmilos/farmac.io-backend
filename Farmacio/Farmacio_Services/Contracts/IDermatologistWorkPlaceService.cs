using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistWorkPlaceService : ICrudService<DermatologistWorkPlace>
    {
        IEnumerable<DermatologistWorkPlace> GetWorkPlacesFor(Guid dermatologistId);
        DermatologistWorkPlace GetWorkPlaceInPharmacyFor(Guid dermatologistId, Guid pharmacyId);
    }
}