using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;

namespace Farmacio_API.Contracts.Requests.PharmacyPriceLists
{
    public class CreateMedicinePriceRequest
    {
        public float Price { get; set; }
        public Guid MedicineId { get; set; }
    }
}