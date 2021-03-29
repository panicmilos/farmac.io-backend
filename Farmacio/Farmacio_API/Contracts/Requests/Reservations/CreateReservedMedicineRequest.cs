using System;

namespace Farmacio_API.Contracts.Requests.Reservations
{
    public class CreateReservedMedicineRequest
    {
        public int Quantity { get; set; }
        public Guid MedicineId { get; set; }
    }
}