using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierOfferService : CrudService<SupplierOffer>, ISupplierOfferService
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierStockService _supplierStockService;
        private readonly IPharmacyStockService _pharmacyStockService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly ICrudService<PharmacyOrder> _pharmacyOrderService;

        public SupplierOfferService(
            ISupplierService supplierService,
            ICrudService<PharmacyOrder> pharmacyOrderService,
            ISupplierStockService supplierStockService,
            IPharmacyStockService pharmacyStockService,
            ITemplatesProvider templatesProvider,
            IEmailDispatcher emailDispatcher,
            IRepository<SupplierOffer> repository) :
            base(repository)
        {
            _supplierService = supplierService;
            _supplierStockService = supplierStockService;
            _pharmacyOrderService = pharmacyOrderService;
            _pharmacyStockService = pharmacyStockService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
        }

        public IEnumerable<SupplierOffer> ReadFor(Guid supplierId)
        {
            return Read().Where(offer => offer.SupplierId == supplierId).ToList();
        }

        public IEnumerable<SupplierOffer> ReadForPharmacyOrder(Guid pharmacyOrderId)
        {
            return Read().Where(offer => offer.PharmacyOrderId == pharmacyOrderId).ToList();
        }

        public SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId)
        {
            return Read().FirstOrDefault(offer => offer.PharmacyOrderId == offerId && offer.SupplierId == supplierId);
        }

        public SupplierOffer AcceptOffer(Guid offerId, Guid pharmacyAdminId)
        {
            var offer = TryToRead(offerId);
            var order = _pharmacyOrderService.TryToRead(offer.PharmacyOrderId);

            if (order.PharmacyAdminId != pharmacyAdminId)
                throw new BadLogicException(
                    "Only the pharmacy admin who has created the offer can accept an offer for it.");
            if (order.IsProcessed)
                throw new BadLogicException("The order has already been processed.");
            if (offer.Status != OfferStatus.WaitingForAnswer)
                throw new BadLogicException("The offer has already been handled.");
            if (order.OffersDeadline > DateTime.Now)
                throw new BadLogicException("The offer deadline has not expired.");

            ReadForPharmacyOrder(offer.PharmacyOrderId)
                .Where(readOffer => readOffer.Id != offerId)
                .ToList()
                .ForEach(RefuseOffer);

            order.OrderedMedicines.ForEach(orderedMedicine =>
            {
                var pharmacyMedicine =
                    _pharmacyStockService.ReadForPharmacy(order.PharmacyId, orderedMedicine.MedicineId);
                if (pharmacyMedicine == null)
                {
                    _pharmacyStockService.Create(new PharmacyMedicine
                    {
                        MedicineId = orderedMedicine.MedicineId,
                        PharmacyId = order.PharmacyId,
                        Quantity = orderedMedicine.Quantity
                    });
                    return;
                }
                pharmacyMedicine.Quantity += orderedMedicine.Quantity;
                _pharmacyStockService.Update(pharmacyMedicine);
            });

            offer.Status = OfferStatus.Accepted;
            var updatedOffer = Update(offer);
            order.IsProcessed = true;
            _pharmacyOrderService.Update(order);

            var supplier = _supplierService.TryToRead(offer.SupplierId);
            var offerAcceptedEmail = _templatesProvider.FromTemplate<Email>("OfferAccepted",
                new { To = supplier.Email, Name = supplier.User.FirstName });
            _emailDispatcher.Dispatch(offerAcceptedEmail);

            return updatedOffer;
        }

        private void RefuseOffer(SupplierOffer otherOffer)
        {
            ReturnMedicinesToStock(otherOffer);
            otherOffer.Status = OfferStatus.Refused;
            Update(otherOffer);
        }

        public SupplierOffer CancelOffer(Guid offerId)
        {
            var existingOffer = TryToRead(offerId);
            if (existingOffer.PharmacyOrder.IsProcessed)
            {
                throw new OrderIsAlreadyProcessedException("You cannot delete offer for the order that has already been processed.");
            }

            if (DateTime.Now > existingOffer.PharmacyOrder.OffersDeadline)
            {
                throw new BadLogicException("You cannot delete offer after offers deadline.");
            }

            ReturnMedicinesToStock(existingOffer);
            base.Delete(offerId);

            return existingOffer;
        }

        private void ReturnMedicinesToStock(SupplierOffer offer)
        {
            offer.PharmacyOrder.OrderedMedicines.ForEach(orderedMedicine =>
            {
                var supplierMedicine =
                    _supplierStockService.ReadMedicineFor(offer.SupplierId, orderedMedicine.MedicineId);
                supplierMedicine.Quantity += orderedMedicine.Quantity;
                _supplierStockService.Update(supplierMedicine);
            });
        }

        public override SupplierOffer Create(SupplierOffer offer)
        {
            var supplier = _supplierService.TryToRead(offer.SupplierId);
            var order = _pharmacyOrderService.TryToRead(offer.PharmacyOrderId);

            if (ReadOfferFor(supplier.Id, order.Id) != null)
            {
                throw new DuplicateOfferException("You cannot give offer for same order twice.");
            }

            if (order.IsProcessed)
            {
                throw new OrderIsAlreadyProcessedException("You cannot give offer for the order that has already been processed.");
            }

            if (offer.DeliveryDeadline > order.OffersDeadline)
            {
                throw new DeliveryDateIsAfterDeadlineException("Delivery date must be before deadline.");
            }

            ValidateSupplierStockForOffer(supplier.Id, order.OrderedMedicines);
            UpdateSupplierStock(supplier.Id, order.OrderedMedicines);

            return base.Create(offer);
        }

        public override SupplierOffer Update(SupplierOffer offer)
        {
            var existingOffer = TryToRead(offer.Id);

            if (existingOffer.PharmacyOrder.IsProcessed)
            {
                throw new OrderIsAlreadyProcessedException("You cannot change offer for the order that has already been processed.");
            }

            if (offer.DeliveryDeadline > existingOffer.PharmacyOrder.OffersDeadline)
            {
                throw new DeliveryDateIsAfterDeadlineException("Delivery date must be before deadline.");
            }

            existingOffer.TotalPrice = offer.TotalPrice;
            existingOffer.DeliveryDeadline = offer.DeliveryDeadline;

            return base.Update(existingOffer);
        }

        private void ValidateSupplierStockForOffer(Guid supplierId, IEnumerable<OrderedMedicine> orderedMedicines)
        {
            var supplierMedicines = _supplierStockService.ReadFor(supplierId);
            orderedMedicines.ToList().ForEach(orderedMedicine =>
            {
                var medicineOfSupplier = supplierMedicines.FirstOrDefault(medicine => medicine.MedicineId == orderedMedicine.MedicineId);
                if (medicineOfSupplier == null || medicineOfSupplier.Quantity < orderedMedicine.Quantity)
                {
                    throw new NotEnoughtMedicinesToGiveAOfferException($"Supplier doesn't have enough {orderedMedicine.Medicine.Name}.");
                }
            });
        }

        private void UpdateSupplierStock(Guid supplierId, IEnumerable<OrderedMedicine> orderedMedicines)
        {
            var supplierMedicines = _supplierStockService.ReadFor(supplierId);
            orderedMedicines.ToList().ForEach(orderedMedicine =>
            {
                var medicineOfSupplier = supplierMedicines.FirstOrDefault(medicine => medicine.MedicineId == orderedMedicine.MedicineId);
                medicineOfSupplier.Quantity -= orderedMedicine.Quantity;
                _supplierStockService.Update(medicineOfSupplier);
            });
        }

        public IEnumerable<SupplierOffer> ReadByStatusFor(Guid supplierId, OfferStatus? status)
        {
            if (status == null)
            {
                return ReadFor(supplierId);
            }

            return ReadFor(supplierId).Where(offer => offer.Status == status);
        }
    }
}