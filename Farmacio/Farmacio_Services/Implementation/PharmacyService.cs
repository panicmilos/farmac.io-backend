using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using System;
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

        public IEnumerable<PharmaciesOfMedicineDTO> MedicineInPharmacies(Guid Id)
        {
            return from pharmacy in base.Read().ToList()
                   from medicinePriceList in pharmacy.PriceList.MedicinePriceList
                         where medicinePriceList.Medicine.Id.Equals(Id)
                         select new PharmaciesOfMedicineDTO
                         {
                             Name = pharmacy.Name,
                             Id = pharmacy.Id,
                             Address = pharmacy.Address,
                             Description = pharmacy.Description,
                             Price = medicinePriceList.Price
                         };
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