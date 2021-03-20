using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class PharmacyOrder : BaseEntity
    {
        public Pharmacy Pharmacy { get; set; }
        public PharmacyAdmin PharmacyAdmin { get; set; }
        public DateTime OffersDeadline { get; set; }
        public bool IsProcessed { get; set; }
        public List<OrderedMedicine> OrderedMedicines { get; set; }
    }

    public class OrderedMedicine : BaseEntity
    {
        public int Quantity { get; set; }
        public Medicine Medicine { get; set; }
    }
}
