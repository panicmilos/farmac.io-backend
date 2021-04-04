using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class PharmacyStockService : CrudService<PharmacyMedicine>, IPharmacyStockService
    {
        public PharmacyStockService(IRepository<PharmacyMedicine> repository) : base(repository)
        {
        }

        public PharmacyMedicine ReadForPharmacy(Guid pharmacyId, Guid medicineId)
        {
            return ReadForPharmacy(pharmacyId).FirstOrDefault(pm => pm.MedicineId == medicineId);
        }

        public IEnumerable<PharmacyMedicine> ReadForPharmacy(Guid pharmacyId)
        {
            return Read().Where(pm => pm.PharmacyId == pharmacyId).ToList();
        }
    }
}