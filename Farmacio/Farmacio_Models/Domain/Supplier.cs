using System;
using System.ComponentModel.DataAnnotations;

namespace Farmacio_Models.Domain
{
    public class Supplier : User
    {
    }

    public class SupplierMedicine : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        [ConcurrencyCheck]
        public int Quantity { get; set; }
    }
}