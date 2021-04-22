using System;
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
        private readonly ICrudService<OrderedMedicine> _orderedMedicineService;
        public PharmacyOrderService(IPharmacyService pharmacyService, IPharmacyAdminService pharmacyAdminService
            , IMedicineService medicineService, ICrudService<OrderedMedicine> orderedMedicineService
            , IRepository<PharmacyOrder> repository) : base(repository)
        {
            _pharmacyService = pharmacyService;
            _pharmacyAdminService = pharmacyAdminService;
            _medicineService = medicineService;
            _orderedMedicineService = orderedMedicineService;
        }

        public override PharmacyOrder Create(PharmacyOrder pharmacyOrder)
        {
            _pharmacyService.TryToRead(pharmacyOrder.PharmacyId);
            if(_pharmacyAdminService.ReadByUserId(pharmacyOrder.PharmacyAdminId) == null)
                throw new MissingEntityException("Pharmacy admin user not found.");
            pharmacyOrder.OrderedMedicines.ForEach(orderedMedicine =>
                _medicineService.TryToRead(orderedMedicine.MedicineId));
            
            return base.Create(pharmacyOrder);
        }

        public override PharmacyOrder Update(PharmacyOrder pharmacyOrder)
        {
            var existingPharmacyOrder = TryToRead(pharmacyOrder.Id);
            _pharmacyService.TryToRead(pharmacyOrder.PharmacyId);
            
            // TODO: Check if supplier offer is created.
            
            if(_pharmacyAdminService.ReadByUserId(pharmacyOrder.PharmacyAdminId) == null)
                throw new MissingEntityException("Pharmacy admin user not found.");
            pharmacyOrder.OrderedMedicines.ForEach(orderedMedicine =>
                _medicineService.TryToRead(orderedMedicine.MedicineId));

            _orderedMedicineService.Delete(
                existingPharmacyOrder.OrderedMedicines.Select(orderedMedicine => orderedMedicine.Id));
            
            existingPharmacyOrder.OffersDeadline = pharmacyOrder.OffersDeadline;
            existingPharmacyOrder.OrderedMedicines = pharmacyOrder.OrderedMedicines;
            
            return base.Update(existingPharmacyOrder);
        }
    }
}