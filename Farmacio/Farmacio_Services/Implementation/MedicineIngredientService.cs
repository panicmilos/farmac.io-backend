using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class MedicineIngredientService : CrudService<MedicineIngredient>, IMedicineIngredientService
    {
        public MedicineIngredientService(IRepository<MedicineIngredient> repository) :
            base(repository)
        {
        }

        public IEnumerable<MedicineIngredient> GetIngredientsFor(Guid medicineId)
        {
            return base.Read()
                .ToList()
                .Where(ingredient => ingredient.MedicineId == medicineId);
        }

        public void UpdateIngridientsFor(Guid medicineId, IEnumerable<MedicineIngredient> ingredients)
        {
            var existingIngredientsFor = GetIngredientsFor(medicineId);

            foreach (var ingredient in ingredients)
            {
                var existingIngredient = existingIngredientsFor.FirstOrDefault(i => i.Name == ingredient.Name);
                if (existingIngredient == null)
                {
                    ingredient.MedicineId = medicineId;
                    Create(ingredient);
                }
                else
                {
                    ingredient.Id = existingIngredient.Id;
                    ingredient.MedicineId = medicineId;
                    existingIngredient.MassInMilligrams = ingredient.MassInMilligrams;
                    Update(existingIngredient);
                }
            }
            foreach (var ingredient in existingIngredientsFor)
            {
                if (ingredients.FirstOrDefault(i => i.Name == ingredient.Name) == null)
                {
                    Delete(ingredient.Id);
                }
            }
        }

        public void DeleteIngridientsFor(Guid medicineId)
        {
            var ingredientsFor = GetIngredientsFor(medicineId).ToList();

            ingredientsFor.ForEach(ingredient => base.Delete(ingredient.Id));
        }
    }
}