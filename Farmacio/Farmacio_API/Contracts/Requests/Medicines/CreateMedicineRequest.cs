using Farmacio_Models.Domain;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class CreateMedicineRequest
    {
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public MedicineForm Form { get; set; }
        public CreateMedicineTypeRequest Type { get; set; }
        public string Manufacturer { get; set; }
        public bool IsRecipeOnly { get; set; }
        public string Contraindications { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecommendedDose { get; set; }
        public List<CreateMedicineIngridientRequest> MedicineIngredients { get; set; }
        public List<CreateMedicineReplacementRequest> Replacements { get; set; }
    }
}