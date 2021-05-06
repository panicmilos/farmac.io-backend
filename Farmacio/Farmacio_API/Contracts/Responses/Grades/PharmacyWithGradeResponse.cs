using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_API.Contracts.Responses.Grades
{
    public class PharmacyWithGradeResponse
    {
        public Pharmacy Pharmacy { get; set; }
        public int Grade { get; set; }
    }
}