using System;

namespace Farmacio_API.Contracts.Requests.LoyaltyPoints
{
    public class UpdateMedicinePointsRequest
    {
        public Guid MedicineId { get; set; }
        public int Points { get; set; }
    }
}