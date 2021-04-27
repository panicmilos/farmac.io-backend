using System;

namespace Farmacio_API.Contracts.Requests.Grades
{
    public class CreateDermatologistGradeRequest
    {
        public int Grade { get; set; }
        public Guid PatientId { get; set; }
        public Guid DermatologistId { get; set; }
    }
}