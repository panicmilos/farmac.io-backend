using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface ISupplierOfferService : ICrudService<SupplierOffer>
    {
        SupplierOffer ReadOfferFor(Guid supplierId, Guid offerId);
    }
}