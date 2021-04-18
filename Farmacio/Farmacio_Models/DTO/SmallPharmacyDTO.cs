using Farmacio_Models.Domain;
using System;

namespace Farmacio_Models.DTO
{
    public class SmallPharmacyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public int AverageGrade { get; set; }
    }
}