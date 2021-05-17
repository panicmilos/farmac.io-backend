namespace Farmacio_Models.DTO
{
    public class PatientSearchParams
    {
        public string Name { get; set; }
        public string SortCriteria { get; set; }
        public bool IsAscending { get; set; }

        public void Deconstruct(out string name, out string sortCriteria, out bool isAscending)
        {
            name = Name;
            sortCriteria = SortCriteria;
            isAscending = IsAscending;
        }
    }
}
