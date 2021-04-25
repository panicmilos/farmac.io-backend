using System;

namespace Farmacio_Models.DTO
{
    public class SmallReservedMedicineDTO
    {
        public Guid MedicineId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
