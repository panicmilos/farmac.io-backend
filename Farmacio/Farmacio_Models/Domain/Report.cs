namespace Farmacio_Models.Domain
{
    public class Report : BaseEntity
    {
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
        public virtual ERecipe Recipe { get; set; }
    }
}
