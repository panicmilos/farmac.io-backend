using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class UpdateMedicineRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MedicineForm Form { get; set; }
        public UpdateMedicineTypeRequest Type { get; set; }
        public string Manufacturer { get; set; }
        public bool IsRecipeOnly { get; set; }
        public string Contraindications { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecommendedDose { get; set; }
        public List<UpdateMedicineIngredientRequest> MedicineIngredients { get; set; }
        public List<UpdateMedicineReplacementRequest> Replacements { get; set; }
    }
}