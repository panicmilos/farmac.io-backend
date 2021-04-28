using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface ISupplierOfferService : ICrudService<SupplierOffer>
    {
        IEnumerable<SupplierOffer> ReadFor(Guid supplierId);

        SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId);

        IEnumerable<SupplierOffer> ReadByStatusFor(Guid supplierId, OfferStatus? status);
    }
}