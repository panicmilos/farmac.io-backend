namespace Farmacio_Models.DTO
{
    public class MedicineSearchParams
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int GradeFrom { get; set; }
        public int GradeTo { get; set; }

        public void Deconstruct(out string name, out string type, out int gradeFrom, out int gradeTo)
        {
            name = Name;
            type = Type;
            gradeFrom = GradeFrom;
            gradeTo = GradeTo;
        }
    }
}