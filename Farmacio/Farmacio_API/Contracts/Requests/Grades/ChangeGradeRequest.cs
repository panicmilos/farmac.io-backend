using System;

namespace Farmacio_API.Contracts.Requests.Grades
{
    public class ChangeGradeRequest
    {
        public int Value { get; set; }
        public Guid GradeId { get; set; }
    }
}