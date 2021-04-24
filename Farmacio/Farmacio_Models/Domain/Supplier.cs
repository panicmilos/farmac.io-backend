using System;

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
        public int Quantity { get; set; }
    }
}