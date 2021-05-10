using Farmacio_Models.Domain;
using System;

namespace Farmacio_Models.DTO
{
    public class PharmacyForERecipeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float AverageGrade { get; set; }
        public Address Address { get; set; }
        public float TotalPriceOfMedicines { get; set; }
    }
}