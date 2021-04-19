using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;

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

        public IEnumerable<PharmacyMedicine> ReadForPharmacyInStock(Guid pharmacyId)
        {
            return ReadForPharmacy(pharmacyId).Where(pharmacyMedicine => pharmacyMedicine.Quantity != 0);
        }

        public override PharmacyMedicine Create(PharmacyMedicine pharmacyMedicine)
        {
            if (ReadForPharmacy(pharmacyMedicine.PharmacyId, pharmacyMedicine.MedicineId) != null)
                throw new PharmacyMedicineAlreadyExistsException("Pharmacy medicine already exists.");
            return base.Create(pharmacyMedicine);
        }
    }
}