using System;

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