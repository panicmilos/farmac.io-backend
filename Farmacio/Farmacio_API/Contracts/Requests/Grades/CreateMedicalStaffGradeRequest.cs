using System;

namespace Farmacio_API.Contracts.Requests.Grades
{
    public class CreateMedicalStaffGradeRequest
    {
        public int Grade { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicalStaffId { get; set; }
    }
}