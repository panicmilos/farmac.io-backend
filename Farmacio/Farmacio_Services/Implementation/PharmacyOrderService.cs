using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacyOrderService : CrudService<PharmacyOrder>, IPharmacyOrderService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacyAdminService _pharmacyAdminService;
        private readonly IMedicineService _medicineService;
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        private readonly ICrudService<OrderedMedicine> _orderedMedicineService;
        public PharmacyOrderService(IPharmacyService pharmacyService, IPharmacyAdminService pharmacyAdminService
            , IMedicineService medicineService, ICrudService<OrderedMedicine> orderedMedicineService
            , IPharmacyPriceListService pharmacyPriceListService, IRepository<PharmacyOrder> repository) : base(repository)
        {
            _pharmacyService = pharmacyService;
            _pharmacyAdminService = pharmacyAdminService;
            _medicineService = medicineService;
            _orderedMedicineService = orderedMedicineService;
            _pharmacyPriceListService = pharmacyPriceListService;
        }

        public override PharmacyOrder Create(PharmacyOrder pharmacyOrder)
        {
            ValidatePharmacyOrder(pharmacyOrder, pharmacyOrder.PharmacyId, pharmacyOrder.PharmacyAdminId);
            
            return base.Create(pharmacyOrder);
        }

        public override PharmacyOrder Update(PharmacyOrder pharmacyOrder)
        {
            var existingPharmacyOrder = TryToRead(pharmacyOrder.Id);
            ValidatePharmacyOrder(pharmacyOrder, existingPharmacyOrder.PharmacyId,
                existingPharmacyOrder.PharmacyAdminId);

            _orderedMedicineService.Delete(
                existingPharmacyOrder.OrderedMedicines.Select(orderedMedicine => orderedMedicine.Id));
            
            existingPharmacyOrder.OffersDeadline = pharmacyOrder.OffersDeadline;
            existingPharmacyOrder.OrderedMedicines = pharmacyOrder.OrderedMedicines;
            
            return base.Update(existingPharmacyOrder);
        }

        public IEnumerable<PharmacyOrder> ReadFor(Guid pharmacyId, bool? isProcessed)
        {
            return Read().Where(pharmacyOrder => pharmacyOrder.PharmacyId == pharmacyId &&
                                                 (isProcessed == null ||
                                                  pharmacyOrder.IsProcessed == isProcessed.Value)).ToList();
        }
        
        private void ValidatePharmacyOrder(PharmacyOrder pharmacyOrder, Guid pharmacyId, Guid pharmacyAdminId)
        {
            _pharmacyService.TryToRead(pharmacyId);

            if (pharmacyOrder.OffersDeadline < DateTime.Now)
                throw new BadLogicException("Offer deadline is in the past.");

            // TODO: Check if supplier offer is created.

            if (_pharmacyAdminService.ReadByUserId(pharmacyAdminId) == null)
                throw new MissingEntityException("Pharmacy admin user not found.");
            pharmacyOrder.OrderedMedicines.ForEach(orderedMedicine =>
            {
                _medicineService.TryToRead(orderedMedicine.MedicineId);
                if (_pharmacyPriceListService.ReadForPharmacy(pharmacyId).MedicinePriceList
                    .FirstOrDefault(medicinePrice => medicinePrice.MedicineId == orderedMedicine.MedicineId) == null)
                    throw new BadLogicException("Ordered medicine is not defined in pharmacy's price list.");
                if (orderedMedicine.Quantity <= 0)
                    throw new BadLogicException("Invalid quantity for medicine order.");
            });
        }
    }
}