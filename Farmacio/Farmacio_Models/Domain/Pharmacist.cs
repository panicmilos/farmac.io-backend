namespace Farmacio_Models.Domain
{
    public class Pharmacist : MedicalStaff
    {
        public Pharmacy Pharmacy { get; set; }
        public WorkTime WorkTime { get; set; }
    }
}
