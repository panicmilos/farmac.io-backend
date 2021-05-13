using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyOrderService : ICrudService<PharmacyOrder>
    {
        IEnumerable<PharmacyOrder> ReadFor(Guid pharmacyId, bool? isProcessed);
        IEnumerable<PharmacyOrder> ReadPageFor(Guid pharmacyId, bool? isProcessed, PageDTO pageDto);
    }
}