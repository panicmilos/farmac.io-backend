using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ERecipeService : CrudService<ERecipe>, IERecipeService
    {
        private readonly IPharmacyService _pharmacyService;

        public ERecipeService(IPharmacyService pharmacyService, IRepository<ERecipe> repository) : base(repository)
        {
            _pharmacyService = pharmacyService;
        }

        public override ERecipe Create(ERecipe eRecipe)
        {
            eRecipe.UniqueId = GetUniqueId();
            return base.Create(eRecipe);
        }

        public IEnumerable<PharmacyForERecipeDTO> FindPharmaciesWithMedicinesFrom(Guid eRecipeId)
        {
            var existingERecipe = TryToRead(eRecipeId);

            return _pharmacyService.Read().ToList()
                .Where(pharmacy =>
                {
                    return existingERecipe.Medicines.All(medicine =>
                    {
                        try
                        {
                            var medicineInPharmacy = _pharmacyService.ReadMedicine(pharmacy.Id, medicine.MedicineId);
                            if (medicineInPharmacy.InStock < medicine.Quantity)
                            {
                                return false;
                            }
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    });
                })
                .Select(pharmacy => new PharmacyForERecipeDTO
                {
                    Name = pharmacy.Name,
                    Address = pharmacy.Address,
                    AverageGrade = pharmacy.AverageGrade,
                    TotalPriceOfMedicines = existingERecipe.Medicines.Sum(medicine => _pharmacyService.ReadMedicine(pharmacy.Id, medicine.MedicineId).Price * medicine.Quantity)
                });
        }

        public IEnumerable<ERecipe> ReadFor(Guid patientId)
        {
            return Read().Where(eRecipe => eRecipe.PatientId == patientId).ToList();
        }

        public bool WasMedicinePrescribedToPatient(Guid patientId, Guid medicineId)
        {
            var eRecipes = ReadFor(patientId);
            eRecipes = eRecipes.Where(eRecipe =>
            {
                var medicines = eRecipe.Medicines;
                return medicines.Where(medicine => medicine.MedicineId == medicineId).FirstOrDefault() != null;
            }).ToList();
            return eRecipes.Count() > 0;
        }

        private string GetUniqueId()
        {
            string uniqueId;
            do
            {
                uniqueId = StringUtils.RandomString(10);
            } while (!IsIdUnique(uniqueId));

            return uniqueId;
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(reservation => reservation.UniqueId == id) == default;
        }
    }
}