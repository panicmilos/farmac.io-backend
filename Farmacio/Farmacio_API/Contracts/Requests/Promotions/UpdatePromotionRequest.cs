using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;

namespace Farmacio_API.Contracts.Requests.Promotions
{
    public class UpdatePromotionRequest
    {
        public Guid Id { get; set; }
        public int Discount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}