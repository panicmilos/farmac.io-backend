namespace Farmacio_Models.Domain
{
    public class Grade : BaseEntity
    {
        // public IGradeable GradeableEntity { get; set; }
        public int Value { get; set; }
        public string PatientId { get; set; }
    }
}
