using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;

namespace Farmacio_API.Contracts.Requests.Promotions
{
    public class CreatePromotionRequest
    {
        public int Discount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid PharmacyId { get; set; }
    }
}