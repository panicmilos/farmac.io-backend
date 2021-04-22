using System;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyPriceListService : ICrudService<PharmacyPriceList>
    {
        PharmacyPriceList ReadForPharmacy(Guid pharmacyId);
        PharmacyPriceList ReadWithMostRecentPricesFor(Guid pharmacyId);
        PharmacyPriceList TryToReadFor(Guid pharmacyId);
        PharmacyPriceList TryToReadWithMostRecentPricesFor(Guid pharmacyId);
    }
}