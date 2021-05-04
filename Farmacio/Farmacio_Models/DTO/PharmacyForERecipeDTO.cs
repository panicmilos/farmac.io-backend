using Farmacio_Models.Domain;

namespace Farmacio_Models.DTO
{
    public class PharmacyForERecipeDTO
    {
        public string Name { get; set; }
        public float AverageGrade { get; set; }
        public Address Address { get; set; }
        public float TotalPriceOfMedicines { get; set; }
    }
}