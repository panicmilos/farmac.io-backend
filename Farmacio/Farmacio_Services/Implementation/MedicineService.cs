using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class MedicineService : CrudService<Medicine>, IMedicineService
    {
        private readonly IMedicineReplacementService _replacementService;
        private readonly IMedicineIngredientService _ingredientService;
        private readonly IPharmacyStockService _pharmacyStockService;
        private readonly IPharmacyService _pharmacyService;
        private readonly ICrudService<Reservation> _reservationService;
        private readonly ICrudService<NotInStock> _notInStockService;

        public MedicineService(IMedicineReplacementService replacementService, IMedicineIngredientService ingredientService,
            IPharmacyService pharmacyService, IPharmacyStockService pharmacyStockService,
            ICrudService<NotInStock> notInStockService, ICrudService<Reservation> reservationService
            , IRepository<Medicine> repository) :
            base(repository)
        {
            _replacementService = replacementService;
            _ingredientService = ingredientService;
            _pharmacyStockService = pharmacyStockService;
            _pharmacyService = pharmacyService;
            _notInStockService = notInStockService;
            _reservationService = reservationService;
        }

        public FullMedicineDTO ReadFullMedicine(Guid id)
        {
            return new FullMedicineDTO
            {
                Medicine = base.Read(id),
                Ingredients = _ingredientService.GetIngredientsFor(id).ToList(),
                Replacements = _replacementService.GetReplacementsFor(id).ToList()
            };
        }

        public FullMedicineDTO Create(FullMedicineDTO fullMedicineDto)
        {
            var medicine = fullMedicineDto.Medicine;
            var ingredients = fullMedicineDto.Ingredients;
            var replacements = fullMedicineDto.Replacements;

            if (!IsIdUnique(medicine.UniqueId))
            {
                throw new BadLogicException("Code must be unique.");
            }

            if (replacements.Any(replacement => Read(replacement.ReplacementMedicineId) == null))
            {
                throw new MissingEntityException("Replacement medicine does not exist in the system.");
            }

            var createdMedicine = base.Create(medicine);
            ingredients.ForEach(ingredient => { ingredient.MedicineId = medicine.Id; _ingredientService.Create(ingredient); });
            replacements.ForEach(replacement => { replacement.MedicineId = medicine.Id; _replacementService.Create(replacement); });

            return fullMedicineDto;
        }

        public override Medicine Update(Medicine medicine)
        {
            var existingMedicine = Read(medicine.Id);
            if (existingMedicine == null)
            {
                throw new MissingEntityException("Medicine does not exist in the system.");
            }

            existingMedicine.Name = medicine.Name;
            existingMedicine.Form = medicine.Form;
            existingMedicine.Type.TypeName = medicine.Type.TypeName;
            existingMedicine.Manufacturer = medicine.Manufacturer;
            existingMedicine.IsRecipeOnly = medicine.IsRecipeOnly;
            existingMedicine.Contraindications = medicine.Contraindications;
            existingMedicine.AdditionalInfo = medicine.AdditionalInfo;
            existingMedicine.RecommendedDose = medicine.RecommendedDose;
            existingMedicine.NumberOfGrades = medicine.NumberOfGrades;
            existingMedicine.AverageGrade = medicine.AverageGrade;

            return base.Update(existingMedicine);
        }

        public FullMedicineDTO Update(FullMedicineDTO fullMedicineDto)
        {
            var medicine = fullMedicineDto.Medicine;
            var ingredients = fullMedicineDto.Ingredients;
            var replacements = fullMedicineDto.Replacements;

            if (replacements.Any(replacement => Read(replacement.ReplacementMedicineId) == null))
            {
                throw new MissingEntityException("Replacement medicine does not exist in the system.");
            }

            fullMedicineDto.Medicine = Update(medicine);
            _replacementService.UpdateReplacementsFor(medicine.Id, replacements);
            _ingredientService.UpdateIngridientsFor(medicine.Id, ingredients);

            return fullMedicineDto;
        }

        public override Medicine Delete(Guid id)
        {
            var medicine = TryToRead(id);

            if(_reservationService.Read().Any(reservation => reservation.State == ReservationState.Reserved &&
                reservation.Medicines.FirstOrDefault(reservedMedicine => 
                    reservedMedicine.MedicineId == medicine.Id) != null))
                throw new BadLogicException("Medicine has been reserved and can't be deleted.");
            
            medicine.Active = false;
            medicine.Type.Active = false;
            base.Update(medicine);

            _ingredientService.DeleteIngridientsFor(medicine.Id);
            _replacementService.DeleteReplacementsFor(medicine.Id);

            return medicine;
        }

        public IEnumerable<SmallMedicineDTO> ReadForDisplay()
        {
            return base.Read()
                .ToList()
                .Select(medicine => new SmallMedicineDTO
                {
                    Id = medicine.Id,
                    Name = medicine.Name,
                    Type = medicine.Type,
                    AverageGrade = medicine.AverageGrade,
                    Manufacturer = medicine.Manufacturer
                });
        }

        public IEnumerable<SmallMedicineDTO> ReadBy(MedicineSearchParams searchParams)
        {
            var (name, type, gradeFrom, gradeTo) = searchParams;

            return ReadForDisplay().Where(m => string.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower()))
                                   .Where(m => string.IsNullOrEmpty(type) || m.Type.TypeName.ToLower() == type.ToLower())
                                   .Where(m => m.AverageGrade >= gradeFrom && m.AverageGrade <= gradeTo);
        }

        public IEnumerable<string> ReadMedicineTypes()
        {
            return Read().ToList().Select(m => m.Type.TypeName.ToLower()).ToHashSet();
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(medicine => medicine.UniqueId == id) == default;
        }

        public IEnumerable<string> ReadMedicineNames(Guid pharmacyId)
        {
            return _pharmacyStockService.ReadForPharmacy(pharmacyId)?.Select(mp => mp.Medicine.Name);
        }

        public IEnumerable<CheckMedicineDTO> ReadMedicinesOrReplacementsByName(Guid pharmacyId, string name)
        {
            var medicines = _pharmacyStockService.ReadForPharmacy(pharmacyId)?
                .Select(mp => mp.Medicine).Where(m => m.Name == name)
                .Select(m =>
                {
                    var medicineInPharmacy = _pharmacyService.ReadMedicine(pharmacyId, m.Id);
                    if (medicineInPharmacy.InStock == 0)
                        _notInStockService.Create(new NotInStock { MedicineId = m.Id, PharmacyId = pharmacyId });
                    return medicineInPharmacy;
                });

            List<MedicineInPharmacyDTO> replacements = new List<MedicineInPharmacyDTO>();
            foreach (var medicine in medicines)
            {
                var replacementsOfMed = _replacementService.GetReplacementsFor(medicine.MedicineId);
                foreach (var replacement in replacementsOfMed)
                {
                    try
                    {
                        var m = _pharmacyService.ReadMedicine(pharmacyId, replacement.ReplacementMedicineId);
                        replacements.Add(m);
                    }
                    catch (MissingEntityException)
                    {
                        continue;
                    }
                }
            }
            var result = medicines.Select(m => new CheckMedicineDTO
            {
                MedicineId = m.MedicineId,
                InStock = m.InStock,
                IsReplacement = false,
                Name = m.Name,
                Price = m.Price
            });
            return result.Concat(replacements.Select(m => new CheckMedicineDTO
            {
                MedicineId = m.MedicineId,
                InStock = m.InStock,
                IsReplacement = true,
                Name = m.Name,
                Price = m.Price
            }));
        }

        public Medicine UpdateGrade(Medicine medicine)
        {
            var existingMedicine = TryToRead(medicine.Id);

            existingMedicine.AverageGrade = medicine.AverageGrade;
            existingMedicine.NumberOfGrades = medicine.NumberOfGrades;

            return base.Update(existingMedicine);
        }

        public IEnumerable<SmallMedicineDTO> ReadPagesToBy(MedicineSearchParams searchParams, PageDTO page)
        {
            return PaginationUtils<SmallMedicineDTO>.PagesTo(ReadBy(searchParams), page);
        }
    }
}