using Farmacio_Models.Domain;
using System;

namespace Farmacio_Models.DTO
{
    public class SmallMedicineDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public MedicineType Type { get; set; }
        public int AverageGrade { get; set; }
        public String Manufacturer { get; set; }
    }
}