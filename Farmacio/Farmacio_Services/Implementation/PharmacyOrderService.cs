using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class PharmacyOrderService : CrudService<PharmacyOrder>, IPharmacyOrderService
    {
        public PharmacyOrderService(IRepository<PharmacyOrder> repository) : base(repository)
        {
        }
    }
}