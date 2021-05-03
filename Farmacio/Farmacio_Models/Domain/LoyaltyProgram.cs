using System;

namespace Farmacio_Models.Domain
{
    public class LoyaltyProgram : BaseEntity
    {
        public String Name { get; set; }
        public int MinPoints { get; set; }
        public int Discount { get; set; }
    }
}
