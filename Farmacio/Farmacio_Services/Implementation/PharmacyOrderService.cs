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
        private readonly ICrudService<SupplierOffer> _supplierOffersService;
        public PharmacyOrderService(ICrudService<SupplierOffer> supplierOffersService
            , IRepository<PharmacyOrder> repository) : base(repository)
        {
            _supplierOffersService = supplierOffersService;
        }

        public override PharmacyOrder Update(PharmacyOrder pharmacyOrder)
        {
            var existingPharmacyOrder = TryToRead(pharmacyOrder.Id);
            if (_supplierOffersService.Read()
                .FirstOrDefault(supplierOffer => supplierOffer.PharmacyOrderId == pharmacyOrder.Id) != null)
                throw new BadLogicException("Supplier offer has been created for the provided pharmacy order.");

            existingPharmacyOrder.OffersDeadline = pharmacyOrder.OffersDeadline;
            existingPharmacyOrder.OrderedMedicines = pharmacyOrder.OrderedMedicines;
            
            return base.Update(existingPharmacyOrder);
        }
    }
}