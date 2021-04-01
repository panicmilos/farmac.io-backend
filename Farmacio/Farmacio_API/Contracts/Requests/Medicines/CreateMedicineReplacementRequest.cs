using System;

namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class CreateMedicineReplacementRequest
    {
        public Guid ReplacementMedicineId { get; set; }
    }
}