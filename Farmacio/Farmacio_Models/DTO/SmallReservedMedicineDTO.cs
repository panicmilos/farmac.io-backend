using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class SmallReservedMedicineDTO
    {
        public Guid MedicineId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
