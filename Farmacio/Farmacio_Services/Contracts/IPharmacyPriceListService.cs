using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyPriceListService : ICrudService<PharmacyPriceList>
    {
        PharmacyPriceList ReadForPharmacy(Guid pharmacyId);
        PharmacyPriceList ReadWithMostRecentPricesFor(Guid pharmacyId);
        PharmacyPriceList TryToReadFor(Guid pharmacyId);
        PharmacyPriceList TryToReadWithMostRecentPricesFor(Guid pharmacyId);

        IEnumerable<string> ReadNamesOfMedicinesIn(Guid pharmacyId);
    }
}