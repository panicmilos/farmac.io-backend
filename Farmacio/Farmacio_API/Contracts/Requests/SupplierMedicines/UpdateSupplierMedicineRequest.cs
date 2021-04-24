using System;

namespace Farmacio_API.Contracts.Requests.SupplierMedicines
{
    public class UpdateSupplierMedicineRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}