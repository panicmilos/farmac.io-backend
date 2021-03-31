using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistWorkPlaceService : ICrudService<DermatologistWorkPlace>
    {
        IEnumerable<DermatologistWorkPlace> GetDermatologistWorkPlaces(Guid dermatologistId);
        DermatologistWorkPlace GetDermatologistWorkPlaceInPharmacy(Guid dermatologistId, Guid pharmacyId);
    }
}