using System;

namespace Farmacio_Models.DTO
{
    public class MedicineInPharmacyDTO
    {
        public Guid MedicineId { get; set; }
        public string Name { get; set; }
        public int InStock { get; set; }
        public float Price { get; set; }
    }
}