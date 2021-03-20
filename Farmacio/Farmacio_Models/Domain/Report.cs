namespace Farmacio_Models.Domain
{
    public class Report : BaseEntity
    {
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
        public ERecipe Recipe { get; set; }
    }
}
