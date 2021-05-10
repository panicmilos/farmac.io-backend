using System;

namespace Farmacio_API.Contracts.Requests.LoyaltyPrograms
{
    public class UpdateLoyaltyProgramRequest
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int MinPoints { get; set; }
        public int Discount { get; set; }
    }
}