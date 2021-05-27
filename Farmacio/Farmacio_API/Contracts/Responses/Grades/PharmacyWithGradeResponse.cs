using Farmacio_Models.Domain;
using System;

namespace Farmacio_API.Contracts.Responses.Grades
{
    public class PharmacyWithGradeResponse
    {
        public Pharmacy Pharmacy { get; set; }
        public int Grade { get; set; }
        public Guid GradeId { get; set; }
    }
}