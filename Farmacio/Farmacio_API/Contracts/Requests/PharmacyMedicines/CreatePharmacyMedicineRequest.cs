using System;

namespace Farmacio_API.Contracts.Requests.PharmacyMedicines
{
    public class CreatePharmacyMedicineRequest
    {
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}