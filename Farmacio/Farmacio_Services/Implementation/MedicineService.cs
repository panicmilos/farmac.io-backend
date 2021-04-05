using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
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

        public MedicineService(IMedicineReplacementService replacementService, IMedicineIngredientService ingredientService,
            IRepository<Medicine> repository) :
            base(repository)
        {
            _replacementService = replacementService;
            _ingredientService = ingredientService;
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
            var medicine = Read(id);
            if (medicine == null)
            {
                throw new MissingEntityException();
            }

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

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(medicine => medicine.UniqueId == id) == default;
        }
    }
}