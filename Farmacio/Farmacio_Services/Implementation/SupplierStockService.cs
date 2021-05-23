using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierStockService : CrudService<SupplierMedicine>, ISupplierStockService
    {
        private readonly ISupplierService _supplierService;
        private readonly IMedicineService _medicineService;
        private readonly ICrudService<SupplierOffer> _supplierOfferService;

        public SupplierStockService(
            ISupplierService supplierService,
            IMedicineService medicineService,
            ICrudService<SupplierOffer> supplierOfferService,
            IRepository<SupplierMedicine> repository) :
            base(repository)
        {
            _supplierService = supplierService;
            _medicineService = medicineService;
            _supplierOfferService = supplierOfferService;
        }

        public override SupplierMedicine Create(SupplierMedicine supplierMedicine)
        {
            _supplierService.TryToRead(supplierMedicine.SupplierId);
            _medicineService.TryToRead(supplierMedicine.MedicineId);

            if (ReadMedicineFor(supplierMedicine.SupplierId, supplierMedicine.MedicineId) != null)
            {
                throw new SupplierMedicineAlreadyExistsException("Supplier already have given medicine in his stock.");
            }

            return base.Create(supplierMedicine);
        }

        public override SupplierMedicine Update(SupplierMedicine supplierMedicine)
        {
            var existingMedicine = TryToRead(supplierMedicine.Id);

            existingMedicine.Quantity = supplierMedicine.Quantity;

            return base.Update(existingMedicine);
        }

        public override SupplierMedicine Delete(Guid id)
        {
            var existingSupplierMedicine = TryToRead(id);

            var isSupplierMedicineInAnyActiveOffer = _supplierOfferService.Read()
                .Where(offer => offer.SupplierId == existingSupplierMedicine.SupplierId && offer.Status == OfferStatus.WaitingForAnswer)
                .ToList()
                .Any(offer => offer.PharmacyOrder.OrderedMedicines.FirstOrDefault(orderedMedicine => orderedMedicine.MedicineId == existingSupplierMedicine.MedicineId) != null);

            if (isSupplierMedicineInAnyActiveOffer)
            {
                throw new BadLogicException("Cannot delete medicine from the stock because it is in some active offer.");
            }

            return base.Delete(id);
        }

        public SupplierMedicine ReadMedicineFor(Guid supplierId, Guid medicineId)
        {
            return Read().FirstOrDefault(supplierMedicine => supplierMedicine.SupplierId == supplierId && supplierMedicine.MedicineId == medicineId);
        }

        public IEnumerable<SupplierMedicine> ReadFor(Guid supplierId)
        {
            return Read().Where(supplierMedicine => supplierMedicine.SupplierId == supplierId).ToList();
        }

        public IEnumerable<SupplierMedicine> ReadPageOfMedicinesFor(Guid supplierId, PageDTO page)
        {
            return PaginationUtils<SupplierMedicine>.Page(ReadFor(supplierId), page);
        }
    }
}