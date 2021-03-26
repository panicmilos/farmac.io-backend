using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class SmallMedicineDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public MedicineType Type { get; set; }
        public int AverageGrade { get; set; }
    }
}
