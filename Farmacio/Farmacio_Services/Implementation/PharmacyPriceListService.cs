using System;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class PharmacyPriceListService : CrudService<PharmacyPriceList>, IPharmacyPriceListService
    {
        public PharmacyPriceListService(IRepository<PharmacyPriceList> repository) : base(repository)
        {
        }

        public PharmacyPriceList ReadForPharmacy(Guid pharmacyId)
        {
            return Read().ToList().FirstOrDefault(pl => pl.PharmacyId == pharmacyId);
        }
    }
}