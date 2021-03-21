using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class PharmacyOrder : BaseEntity
    {
        public virtual Pharmacy Pharmacy { get; set; }
        public virtual PharmacyAdmin PharmacyAdmin { get; set; }
        public DateTime OffersDeadline { get; set; }
        public bool IsProcessed { get; set; }
        public virtual List<OrderedMedicine> OrderedMedicines { get; set; }
    }

    public class OrderedMedicine : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
