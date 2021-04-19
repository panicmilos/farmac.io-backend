using System;

namespace Farmacio_Models.DTO
{
    public class MedicalStaffFilterParamsDTO
    {
        public string PharmacyId { get; set; }
        public string Name { get; set; }
        public int GradeFrom { get; set; }
        public int GradeTo { get; set; } 
        
        public void Deconstruct(out string name, out string pharmacyId, out int gradeFrom, out int gradeTo)
        {
            name = Name;
            pharmacyId = PharmacyId;
            gradeFrom = GradeFrom;
            gradeTo = GradeTo;
        }
    }
}