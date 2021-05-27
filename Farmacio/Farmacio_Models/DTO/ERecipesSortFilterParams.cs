namespace Farmacio_Models.DTO
{
    public class ERecipesSortFilterParams
    {
        public string SortCriteria { get; set; }
        public bool IsAsc { get; set; }
        public bool? IsUsed { get; set; }

        public void Deconstruct(out string sortCriteria, out bool isAsc, out bool? isUsed)
        {
            sortCriteria = SortCriteria;
            isAsc = IsAsc;
            isUsed = IsUsed;
        }
    }
}
