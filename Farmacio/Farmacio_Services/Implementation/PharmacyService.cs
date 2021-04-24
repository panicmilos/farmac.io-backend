﻿using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Services.Implementation.Utils;

namespace Farmacio_Services.Implementation
{
    public class PharmacyService : CrudService<Pharmacy>, IPharmacyService
    {
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        private readonly IPharmacyStockService _pharmacyStockService;

        public PharmacyService(IPharmacyPriceListService pharmacyPriceListService, IPharmacyStockService pharmacyStockService
            , IRepository<Pharmacy> repository) :
            base(repository)
        {
            _pharmacyPriceListService = pharmacyPriceListService;
            _pharmacyStockService = pharmacyStockService;
        }

        public IEnumerable<PharmaciesOfMedicineDTO> MedicineInPharmacies(Guid medicineId)
        {
            var listOfPharmacies = new List<PharmaciesOfMedicineDTO>();
            foreach (var pharmacy in base.Read().ToList())
            {
                var medicinePrice = GetMedicinePriceInPharmacy(medicineId, pharmacy.Id);
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
                    Address = pharmacy.Address,
                    AverageGrade = pharmacy.AverageGrade
                });
        }

        public MedicineInPharmacyDTO ReadMedicine(Guid pharmacyId, Guid medicineId)
        {
            var pharmacy = TryToRead(pharmacyId);

            var medicineStock = _pharmacyStockService.ReadForPharmacy(pharmacyId, medicineId);
            if (medicineStock == null)
                throw new MissingEntityException("Given pharmacy does not have wanted medicine.");

            var medicinePrice = GetMedicinePriceInPharmacy(medicineId, pharmacyId);
            if (medicinePrice == null)
                throw new MissingEntityException("Medicine price not found.");

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
            var pharmacy = TryToRead(pharmacyId);

            var medicineStock = _pharmacyStockService.ReadForPharmacy(pharmacyId, medicineId);
            if (medicineStock == null)
                throw new MissingEntityException("Given pharmacy does not have wanted medicine.");

            medicineStock.Quantity += changeFor;

            base.Update(pharmacy);
        }

        public override Pharmacy Update(Pharmacy pharmacy)
        {
            var existingPharmacy = TryToRead(pharmacy.Id);

            existingPharmacy.Name = pharmacy.Name;
            existingPharmacy.Description = pharmacy.Description;

            existingPharmacy.Address.State = pharmacy.Address.State;
            existingPharmacy.Address.City = pharmacy.Address.City;
            existingPharmacy.Address.StreetName = pharmacy.Address.StreetName;
            existingPharmacy.Address.StreetNumber = pharmacy.Address.StreetNumber;
            existingPharmacy.Address.Lat = pharmacy.Address.Lat;
            existingPharmacy.Address.Lng = pharmacy.Address.Lng;

            return base.Update(existingPharmacy);
        }
        
        private MedicinePrice GetMedicinePriceInPharmacy(Guid medicineId, Guid pharmacyId)
        {
            return _pharmacyPriceListService.ReadForPharmacy(pharmacyId)?
                .MedicinePriceList.Where(mp => mp.MedicineId == medicineId)
                .OrderByDescending(mp => mp.ActiveFrom)
                .FirstOrDefault();
        }

        public IEnumerable<SmallPharmacyDTO> ReadBy(PharmacySearchParams searchParams)
        {
            var (name, streetAndCity, sortCriteria, isAscending, gradeFrom, gradeTo, distanceFrom, distanceTo, userLat, userLon) = searchParams;
            var pharmacies = ReadForHomePage().Where(pharmacy => string.IsNullOrEmpty(name) || pharmacy.Name.ToLower().Contains(name.ToLower()))
                .Where(pharmacy => string.IsNullOrEmpty(streetAndCity) || streetAndCity.ToLower().Contains(pharmacy.Address.City.ToLower()))
                .Where(pharmacy => string.IsNullOrEmpty(streetAndCity) || streetAndCity.ToLower().Contains(pharmacy.Address.StreetName.ToLower()))
                .Where(pharmacy => pharmacy.AverageGrade >= gradeFrom && pharmacy.AverageGrade <= gradeTo);


            if(distanceFrom != 0 && distanceTo != 0 && LatLngUtils.IsLatValid(userLat) && LatLngUtils.IsLngValid(userLon))
            {
                pharmacies = pharmacies.Where(pharmacy =>
                {
                    var distance = LatLngUtils.GetDistanceFromLatLngInKm(new CoordinatesDTO 
                    { Lat = pharmacy.Address.Lat, Lng = pharmacy.Address.Lng }, 
                    new CoordinatesDTO { Lat = userLat, Lng = userLon });
                    return distance >= distanceFrom && distance <= distanceTo;
                });
            }

            var sortingCriteria = new Dictionary<string, Func<SmallPharmacyDTO, object>>()
            {
                { "name", p => p.Name },
                { "city", p => p.Address.City },
                { "grade", p => p.AverageGrade }
            };

            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortingCriterion))
            {
                pharmacies = isAscending ? pharmacies.OrderBy(sortingCriterion) : pharmacies.OrderByDescending(sortingCriterion);
            }

            return pharmacies;
        }

        public IEnumerable<PharmacyDTO> GetPharmaciesOfPharmacists(List<Account> pharmacists, PharmaciesForAppointmentsSearchParams searchParams)
        {
            var pharmacies = new List<PharmacyDTO>();
            foreach (var pharmacistAccount in pharmacists)

            {
                var pharmacist = (Pharmacist)pharmacistAccount.User;
                Console.WriteLine(pharmacist.Pharmacy.Name);
                if (pharmacies.Where(pharmacy => pharmacy.Id == pharmacist.PharmacyId).FirstOrDefault() == null)
                {
                    pharmacies.Add(new PharmacyDTO
                    {
                        Name = pharmacist.Pharmacy.Name,
                        Id = pharmacist.Pharmacy.Id,
                        Address = pharmacist.Pharmacy.Address,
                        AverageGrade = pharmacist.Pharmacy.AverageGrade,
                        Description = pharmacist.Pharmacy.Description,
                        ConsultationPrice = _pharmacyPriceListService.ReadForPharmacy(pharmacist.PharmacyId).ConsultationPrice
                    });
                }
            }

            string sortCriteria = searchParams.SortCriteria;
            bool isAscending = searchParams.IsAsc;
            Console.WriteLine(sortCriteria);
            Console.WriteLine(isAscending);

            var sortingCriteria = new Dictionary<string, Func<PharmacyDTO, object>>()
            {
                { "price", p => p.ConsultationPrice },
                { "grade", p => p.AverageGrade }
            };

            Console.WriteLine(pharmacies.Count);

            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortingCriterion))
            {
                pharmacies = isAscending ? pharmacies.OrderBy(sortingCriterion).ToList() : pharmacies.OrderByDescending(sortingCriterion).ToList();
            }

            Console.WriteLine(pharmacies.Count);

            return pharmacies;
        }
    }
}