using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierOfferService : CrudService<SupplierOffer>, ISupplierOfferService
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierStockService _supplierStockService;
        private readonly ICrudService<PharmacyOrder> _pharmacyOrderService;

        public SupplierOfferService(
            ISupplierService supplierService,
            ICrudService<PharmacyOrder> pharmacyOrderService,
            ISupplierStockService supplierStockService,
            IRepository<SupplierOffer> repository) :
            base(repository)
        {
            _supplierService = supplierService;
            _supplierStockService = supplierStockService;
            _pharmacyOrderService = pharmacyOrderService;
        }

        public IEnumerable<SupplierOffer> ReadFor(Guid supplierId)
        {
            return Read().Where(offer => offer.SupplierId == supplierId).ToList();
        }

        public SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId)
        {
            return Read().FirstOrDefault(offer => offer.PharmacyOrderId == offerId && offer.SupplierId == supplierId);
        }

        public override SupplierOffer Create(SupplierOffer offer)
        {
            var supplier = _supplierService.TryToRead(offer.SupplierId);
            var order = _pharmacyOrderService.TryToRead(offer.PharmacyOrderId);

            if (ReadOfferFor(supplier.Id, order.Id) != null)
            {
                throw new DuplicateOfferException("You can not give offer for same order twice.");
            }

            if (order.IsProcessed)
            {
                throw new OrderIsAlreadyProcessedException("You can not give offer for the order that has already been processed.");
            }

            if (offer.DeliveryDeadline > order.OffersDeadline)
            {
                throw new DeliveryDateIsAfterDeadlineException("Delivery date must be before deadline.");
            }

            ValidateSupplierStockForOffer(supplier.Id, order.OrderedMedicines);
            UpdateSupplierStock(supplier.Id, order.OrderedMedicines);

            return base.Create(offer);
        }

        private void ValidateSupplierStockForOffer(Guid supplierId, IEnumerable<OrderedMedicine> orderedMedicines)
        {
            var supplierMedicines = _supplierStockService.ReadFor(supplierId);
            foreach (var orderedMedicine in orderedMedicines)
            {
                var medicineOfSupplier = supplierMedicines.FirstOrDefault(medicine => medicine.MedicineId == orderedMedicine.MedicineId);
                if (medicineOfSupplier == null || medicineOfSupplier.Quantity < orderedMedicine.Quantity)
                {
                    throw new NotEnoughtMedicinesToGiveAOfferException($"Supplier doesn't have enough {orderedMedicine.Medicine.Name}.");
                }
            }
        }

        private void UpdateSupplierStock(Guid supplierId, IEnumerable<OrderedMedicine> orderedMedicines)
        {
            var supplierMedicines = _supplierStockService.ReadFor(supplierId);
            foreach (var orderedMedicine in orderedMedicines)
            {
                var medicineOfSupplier = supplierMedicines.FirstOrDefault(medicine => medicine.MedicineId == orderedMedicine.MedicineId);
                medicineOfSupplier.Quantity -= orderedMedicine.Quantity;
                _supplierStockService.Update(medicineOfSupplier);
            }
        }
    }
}