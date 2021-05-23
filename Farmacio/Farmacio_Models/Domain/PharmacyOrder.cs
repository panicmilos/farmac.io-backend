using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Farmacio_Models.Domain
{
    public class PharmacyOrder : BaseEntity
    {
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        public Guid PharmacyAdminId { get; set; }
        public virtual PharmacyAdmin PharmacyAdmin { get; set; }
        public DateTime OffersDeadline { get; set; }
        [ConcurrencyCheck]
        public bool IsProcessed { get; set; }
        public virtual List<OrderedMedicine> OrderedMedicines { get; set; }
    }

    public class OrderedMedicine : BaseEntity
    {
        public Guid PharmacyOrderId { get; set; }
        public int Quantity { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
