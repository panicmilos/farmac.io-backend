using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface ISupplierStockService : ICrudService<SupplierMedicine>
    {
        SupplierMedicine ReadMedicineFor(Guid supplierId, Guid medicineId);

        IEnumerable<SupplierMedicine> ReadFor(Guid supplierId);
    }
}