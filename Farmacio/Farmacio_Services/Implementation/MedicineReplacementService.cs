using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class MedicineReplacementService : CrudService<MedicineReplacement>, IMedicineReplacementService
    {
        public MedicineReplacementService(IRepository<MedicineReplacement> repository) :
            base(repository)
        {
        }
    }
}