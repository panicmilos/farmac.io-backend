using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Farmacio_API.Examples.Examples
{
    public class CreateMedicineRequestExample : IExamplesProvider<CreateMedicineRequest>
    {
        public CreateMedicineRequest GetExamples()
        {
            return new CreateMedicineRequest
            {
                Name = "Kafetin gold",
                UniqueId = "KAFETINGLD",
                Form = MedicineForm.Tablet,
                Type = new CreateMedicineTypeRequest
                {
                    TypeName = "Lek za glavu, stvarno ne znam sta je..."
                },
                Manufacturer = "Alkaloid",
                IsRecipeOnly = false,
                Contraindications = "Mogu da se raspisem ali me mrzi...",
                AdditionalInfo = "A samo se nemoj kljukas previse",
                RecommendedDose = "Do 2 dnevno osim ako te stvarnoo ne boli glava",
                MedicineIngredients = new List<CreateMedicineIngridientRequest>
                {
                    new CreateMedicineIngridientRequest
                    {
                        Name = "Ingridient 1",
                        MassInMilligrams = 30
                    },
                    new CreateMedicineIngridientRequest
                    {
                        Name = "Ingridient 2",
                        MassInMilligrams = 60
                    }
                }
            };
        }
    }
}