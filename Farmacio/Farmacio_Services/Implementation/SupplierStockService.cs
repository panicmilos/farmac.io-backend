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

        public override SupplierMedicine Create(SupplierMedicine medicine)
        {
            _supplierService.TryToRead(medicine.SupplierId);
            _medicineService.TryToRead(medicine.MedicineId);

            if (ReadMedicineFor(medicine.SupplierId, medicine.MedicineId) != null)
            {
                throw new SupplierMedicineAlreadyExistsException("Supplier already have given medicine in his stock.");
            }

            return base.Create(medicine);
        }

        public override SupplierMedicine Update(SupplierMedicine medicine)
        {
            var existingMedicine = TryToRead(medicine.Id);

            existingMedicine.Quantity = medicine.Quantity;

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
            return Read().FirstOrDefault(medicine => medicine.SupplierId == supplierId && medicine.MedicineId == medicineId);
        }

        public IEnumerable<SupplierMedicine> ReadFor(Guid supplierId)
        {
            return Read().Where(medicine => medicine.SupplierId == supplierId).ToList();
        }
    }
}