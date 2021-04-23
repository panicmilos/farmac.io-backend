using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyOrderService : ICrudService<PharmacyOrder>
    {
        IEnumerable<PharmacyOrder> ReadFor(Guid pharmacyId, bool? isProcessed);
    }
}