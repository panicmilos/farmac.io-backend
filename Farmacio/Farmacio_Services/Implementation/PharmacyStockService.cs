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
        private readonly ICrudService<PharmacyOrder> _pharmacyOrderService;

        public PharmacyStockService(
            IPharmacyPriceListService pharmacyPriceListService,
            ICrudService<PharmacyOrder> pharmacyOrderService,
            IRepository<PharmacyMedicine> repository) : base(repository)
        {
            _pharmacyPriceListService = pharmacyPriceListService;
            _pharmacyOrderService = pharmacyOrderService;
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
            return base.Create(pharmacyMedicine);
        }

        public override PharmacyMedicine Delete(Guid id)
        {
            var existingPharmacyMedicine = TryToRead(id);

            var isInActiveOrder = _pharmacyOrderService.Read()
                .Where(order => order.IsProcessed == false && order.PharmacyId == existingPharmacyMedicine.PharmacyId)
                .ToList()
                .Any(order => order.OrderedMedicines.FirstOrDefault(orderedMedicine => orderedMedicine.MedicineId == existingPharmacyMedicine.MedicineId) != null);

            if (isInActiveOrder)
            {
                throw new BadLogicException("You cannot delete this medicine from the stock because it is in some active order.");
            }

            return base.Delete(id);
        }
    }
}