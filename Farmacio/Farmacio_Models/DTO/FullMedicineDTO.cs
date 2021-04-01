using Farmacio_Models.Domain;
using System.Collections.Generic;

namespace Farmacio_Models.DTO
{
    public class FullMedicineDTO
    {
        public Medicine Medicine { get; set; }
        public List<MedicineReplacement> Replacements { get; set; }
    }
}