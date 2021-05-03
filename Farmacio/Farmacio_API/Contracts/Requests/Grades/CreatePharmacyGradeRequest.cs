using System;

namespace Farmacio_API.Contracts.Requests.Grades
{
    public class CreatePharmacyGradeRequest
    {
        public int Value { get; set; }
        public Guid PatientId { get; set; }
        public Guid PharmacyId { get; set; }
    }
}