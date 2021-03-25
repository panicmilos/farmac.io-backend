using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PharmacyService : CrudService<Pharmacy>, IPharmacyService
    {
        public PharmacyService(IRepository<Pharmacy> repository) :
            base(repository)
        {
        }

        public IEnumerable<SmallPharmacyDTO> ReadForHomePage()
        {
            return base.Read()
                .ToList()
                .Select(pharmacy => new SmallPharmacyDTO
                {
                    Id = pharmacy.Id,
                    Name = pharmacy.Name,
                    Description = pharmacy.Description,
                    Address = pharmacy.Address
                });
        }
    }
}