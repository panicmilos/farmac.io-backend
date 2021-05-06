using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_API.Contracts.Responses.Grades
{
    public class MedicalStafftWithGradeResponse
    {
        public Account MedicalStaff { get; set; }
        public int Grade { get; set; }
    }
}