using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierStockService : CrudService<SupplierMedicine>, ISupplierStockService
    {
        private readonly ISupplierService _supplierService;
        private readonly IMedicineService _medicineService;

        public SupplierStockService(
            ISupplierService supplierService,
            IMedicineService medicineService,
            IRepository<SupplierMedicine> repository) :
            base(repository)
        {
            _supplierService = supplierService;
            _medicineService = medicineService;
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
            // TODO: Ovde treba cela logika da ne moze da se brise u slucaju
            // da postoji lekova u npr aktivnom oferu
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
    }
}