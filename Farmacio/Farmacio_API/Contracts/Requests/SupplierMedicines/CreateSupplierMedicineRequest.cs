using System;

namespace Farmacio_API.Contracts.Requests.SupplierMedicines
{
    public class CreateSupplierMedicineRequest
    {
        public Guid SupplierId { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}