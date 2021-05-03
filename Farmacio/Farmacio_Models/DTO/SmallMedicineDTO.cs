using Farmacio_Models.Domain;
using System;

namespace Farmacio_Models.DTO
{
    public class SmallMedicineDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MedicineType Type { get; set; }
        public float AverageGrade { get; set; }
        public string Manufacturer { get; set; }
    }
}