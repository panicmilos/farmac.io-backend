using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class DermatologistWorkPlaceService : CrudService<DermatologistWorkPlace>, IDermatologistWorkPlaceService
    {
        public DermatologistWorkPlaceService(IRepository<DermatologistWorkPlace> repository) : base(repository)
        {
        }

        public IEnumerable<DermatologistWorkPlace> GetWorkPlacesFor(Guid dermatologistId)
        {
            return Read().Where(wp => wp.DermatologistId == dermatologistId).ToList();
        }

        public DermatologistWorkPlace GetWorkPlaceInPharmacyFor(Guid dermatologistId, Guid pharmacyId)
        {
            return Read().FirstOrDefault(wp => wp.DermatologistId == dermatologistId && wp.PharmacyId == pharmacyId);
        }
    }
}