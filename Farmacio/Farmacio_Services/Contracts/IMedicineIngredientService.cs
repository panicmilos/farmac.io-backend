using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineIngredientService : ICrudService<MedicineIngredient>
    {
        IEnumerable<MedicineIngredient> GetIngredientsFor(Guid medicineId);

        void UpdateIngridientsFor(Guid medicineId, IEnumerable<MedicineIngredient> ingredients);

        void DeleteIngridientsFor(Guid medicineId);
    }
}