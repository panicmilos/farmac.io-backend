using System;

namespace Farmacio_Models.Domain
{
    public class Grade : BaseEntity
    {
        public int Value { get; set; }
        public Guid PatientId { get; set; }
    }

    public class MedicalStaffGrade : Grade
    {
        public Guid MedicalStaffId { get; set; }
    }

    public class PharmacyGrade : Grade
    {
        public Guid PharmacyId { get; set; }
    }

    public class MedicineGrade : Grade
    {
        public Guid MedicineId { get; set; }
    }
}
