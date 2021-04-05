using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateMedicineRequestExample : IExamplesProvider<UpdateMedicineRequest>
    {
        public UpdateMedicineRequest GetExamples()
        {
            return new UpdateMedicineRequest
            {
                Id = new Guid("08d8f563-8f91-4272-8d1d-e77c3eecb942"),
                Name = "Kafetin gold izmenjen",
                Form = MedicineForm.Capsule,
                Type = new UpdateMedicineTypeRequest
                {
                    TypeName = "Lek za glavu, ali i dalje stvarno ne znam sta je..."
                },
                Manufacturer = "Alkaloid Skoplje",
                IsRecipeOnly = true,
                Contraindications = "Mogu da se raspisem ali me mrzi... I nakon izmene me i dalje mrzi.",
                AdditionalInfo = "A samo se nemoj kljukas previse. I dalje nemoj kljukas mnogo.",
                RecommendedDose = "Do 2 dnevno osim ako te stvarnoo ne boli glava. Ako te stvarnooo boli.",
                MedicineIngredients = new List<UpdateMedicineIngredientRequest>
                {
                    new UpdateMedicineIngredientRequest
                    {
                        Name = "Ingridient 1",
                        MassInMilligrams = 50
                    },
                    new UpdateMedicineIngredientRequest
                    {
                        Name = "Ingridient 3",
                        MassInMilligrams = 30
                    }
                }
            };
        }
    }
}