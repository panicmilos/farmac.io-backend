using System;

namespace Farmacio_API.Contracts.Requests.Grades
{
    public class CreateMedicineGradeRequest
    {
        public int Value { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicineId { get; set; }
    }
}