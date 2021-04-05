using System;

namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class UpdateMedicineReplacementRequest
    {
        public Guid ReplacementMedicineId { get; set; }
    }
}