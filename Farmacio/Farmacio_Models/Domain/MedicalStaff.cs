namespace Farmacio_Models.Domain
{
    public abstract class MedicalStaff : User
    {
        public float AverageGrade { get; set; }
        public int NumberOfGrades { get; set; }
    }
}
