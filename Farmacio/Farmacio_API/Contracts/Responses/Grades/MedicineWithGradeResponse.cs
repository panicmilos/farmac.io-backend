using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Responses.Grades
{
    public class MedicineWithGradeResponse
    {
        public Medicine Medicine { get; set; }
        public int Grade { get; set; }
        public Guid GradeId { get; set; }
    }
}