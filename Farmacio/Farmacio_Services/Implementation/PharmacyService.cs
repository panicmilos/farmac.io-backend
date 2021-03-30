﻿using Farmacio_Models.Domain;
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

            var listOfPharmacies = new List<PharmaciesOfMedicineDTO>();
            foreach(var pharmacy in base.Read().ToList())
            {
                var medicinePrice = pharmacy.PriceList
                    .MedicinePriceList.Where(medicinePrice => medicinePrice.MedicineId == Id)
                    .OrderByDescending(medicinePrice => medicinePrice.ActiveFrom)
                   .FirstOrDefault();
                
                if (medicinePrice == null)
                    continue;

                listOfPharmacies.Add(new PharmaciesOfMedicineDTO()
                {
                    Name = pharmacy.Name,
                    Id = pharmacy.Id,
                    Address = pharmacy.Address,
                    Description = pharmacy.Description,
                    Price = medicinePrice.Price
                });
            }
            return listOfPharmacies.AsEnumerable();
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

        public override Pharmacy Update(Pharmacy entity)
        {
            var pharmacy = Read(entity.Id);
            if (pharmacy == null)
            {
                throw new MissingEntityException("Given pharmacy does not exist in the system.");
            }

            pharmacy.Name = entity.Name;
            pharmacy.Description = entity.Description;

            pharmacy.Address.State = entity.Address.State;
            pharmacy.Address.City = entity.Address.City;
            pharmacy.Address.StreetName = entity.Address.StreetName;
            pharmacy.Address.StreetNumber = entity.Address.StreetNumber;
            pharmacy.Address.Lat = entity.Address.Lat;
            pharmacy.Address.Lng = entity.Address.Lng;
            
            return base.Update(entity);
        }
    }
}