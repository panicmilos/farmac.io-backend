using System;

namespace Farmacio_Models.Domain
{
    public class Grade : BaseEntity
    {
        public int Value { get; set; }
        public Guid PatientId { get; set; }
    }
}
