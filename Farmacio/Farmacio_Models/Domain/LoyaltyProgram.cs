namespace Farmacio_Models.Domain
{
    public class LoyaltyProgram : BaseEntity
    {
        public LoyaltyProgramType Type { get; set; }
        public int MinPoints { get; set; }
        public int Discount { get; set; }
    }

    public enum LoyaltyProgramType
    {
        Regular,
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond
    }
}
