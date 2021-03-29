using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
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

        public MedicineInPharmacyDTO ReadMedicine(Guid pharmacyId, Guid medicineId)
        {
            var pharmacy = Read(pharmacyId);
            if (pharmacy == null)
            {
                throw new MissingEntityException("Given pharmacy does not exist in the system.");
            }

            var medicineStock = pharmacy.Stock.FirstOrDefault(medicine => medicine.MedicineId == medicineId);
            if (medicineStock == null)
            {
                throw new MissingEntityException("Given pharmacy does not have wanted medicine.");
            }

            var medicinePrice = pharmacy.PriceList
                .MedicinePriceList
                .Where(medicinePrice => medicinePrice.MedicineId == medicineId)
                .OrderByDescending(medicinePrice => medicinePrice.ActiveFrom)
                .FirstOrDefault();

            return new MedicineInPharmacyDTO
            {
                MedicineId = medicineId,
                Name = medicineStock.Medicine.Name,
                InStock = medicineStock.Quantity,
                Price = medicinePrice.Price
            };
        }

        public void ChangeStockFor(Guid pharmacyId, Guid medicineId, int changeFor)
        {
            var pharmacy = Read(pharmacyId);
            if (pharmacy == null)
            {
                throw new MissingEntityException("Given pharmacy does not exist in the system.");
            }

            var medicineStock = pharmacy.Stock.FirstOrDefault(medicine => medicine.MedicineId == medicineId);
            if (medicineStock == null)
            {
                throw new MissingEntityException("Given pharmacy does not have wanted medicine.");
            }

            medicineStock.Quantity += changeFor;

            base.Update(pharmacy);
        }
    }
}