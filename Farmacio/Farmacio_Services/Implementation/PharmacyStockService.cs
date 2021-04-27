using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacyStockService : CrudService<PharmacyMedicine>, IPharmacyStockService
    {
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        public PharmacyStockService(IPharmacyPriceListService pharmacyPriceListService
            , IRepository<PharmacyMedicine> repository) : base(repository)
        {
            _pharmacyPriceListService = pharmacyPriceListService;
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

        public IEnumerable<PharmacyMedicine> SearchForPharmacyInStock(Guid pharmacyId, string name)
        {
            return ReadForPharmacyInStock(pharmacyId)
                .Where(pharmacyMedicine => name == null || pharmacyMedicine.Medicine.Name.Contains(name));
        }

        public override PharmacyMedicine Create(PharmacyMedicine pharmacyMedicine)
        {
            if (ReadForPharmacy(pharmacyMedicine.PharmacyId, pharmacyMedicine.MedicineId) != null)
                throw new PharmacyMedicineAlreadyExistsException("Pharmacy medicine already exists.");
            if(!_pharmacyPriceListService.TryToReadFor(pharmacyMedicine.PharmacyId).MedicinePriceList.Exists(
                medicinePrice => medicinePrice.MedicineId == pharmacyMedicine.MedicineId))
                throw new MissingEntityException("Medicine is not defined in pharmacy price list.");
            return base.Create(pharmacyMedicine);
        }
    }
}