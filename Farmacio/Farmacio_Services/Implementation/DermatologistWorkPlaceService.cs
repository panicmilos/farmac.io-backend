using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class DermatologistWorkPlaceService : CrudService<DermatologistWorkPlace>, IDermatologistWorkPlaceService
    {
        public DermatologistWorkPlaceService(IDermatologistWorkPlacesRepository repository) : base(repository)
        {
        }

        public IEnumerable<DermatologistWorkPlace> GetWorkPlacesFor(Guid dermatologistId)
        {
            return ((DermatologistWorkPlacesRepository) _repository).ReadForDermatologist(dermatologistId);
        }

        public DermatologistWorkPlace GetWorkPlaceInPharmacyFor(Guid dermatologistId, Guid pharmacyId)
        {
            return ((DermatologistWorkPlacesRepository) _repository).ReadForDermatologistInPharmacy(dermatologistId,
                pharmacyId);
        }
    }
}