using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Responses.Grades
{
    public class MedicalStafftWithGradeResponse
    {
        public MedicalStaff MedicalStaff { get; set; }
        public int Grade { get; set; }
        public Guid GradeId { get; set; }
    }
}