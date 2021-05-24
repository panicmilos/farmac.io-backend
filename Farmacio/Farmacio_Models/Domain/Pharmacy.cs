using System;
using System.ComponentModel.DataAnnotations;

namespace Farmacio_Models.Domain
{
    public class Pharmacy : BaseEntity
    {
        public string Name { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string Description { get; set; }
        public float AverageGrade { get; set; }
        public int NumberOfGrades { get; set; }
    }

    public class PharmacyMedicine : BaseEntity
    {
        public Guid PharmacyId { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
        
        [ConcurrencyCheck]
        public int Quantity { get; set; }
    }
}