using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface ISupplierOfferService : ICrudService<SupplierOffer>
    {
        IEnumerable<SupplierOffer> ReadFor(Guid supplierId);
        IEnumerable<SupplierOffer> ReadForPharmacyOrder(Guid pharmacyOrderId);

        SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId);
        SupplierOffer AcceptOffer(Guid offerId);
    }
}