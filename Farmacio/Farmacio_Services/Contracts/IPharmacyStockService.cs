using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyStockService : ICrudService<PharmacyMedicine>
    {
        PharmacyMedicine ReadForPharmacy(Guid pharmacyId, Guid medicineId);
        IEnumerable<PharmacyMedicine> ReadForPharmacy(Guid pharmacyId);
        IEnumerable<PharmacyMedicine> ReadForPharmacyInStock(Guid pharmacyId);
        IEnumerable<PharmacyMedicine> SearchForPharmacyInStock(Guid pharmacyId, string name);
    }
}