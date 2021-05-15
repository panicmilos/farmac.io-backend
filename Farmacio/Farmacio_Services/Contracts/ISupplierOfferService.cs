using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface ISupplierOfferService : ICrudService<SupplierOffer>
    {
        IEnumerable<SupplierOffer> ReadFor(Guid supplierId);

        IEnumerable<SupplierOffer> ReadForPharmacyOrder(Guid pharmacyOrderId);

        SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId);

        SupplierOffer AcceptOffer(Guid offerId, Guid pharmacyAdminId);

        SupplierOffer CancelOffer(Guid offerId);

        IEnumerable<SupplierOffer> ReadByStatusFor(Guid supplierId, OfferStatus? status);

        IEnumerable<SupplierOffer> ReadPageOfOffersByStatusFor(Guid supplierId, OfferStatus? status, PageDTO page);
    }
}