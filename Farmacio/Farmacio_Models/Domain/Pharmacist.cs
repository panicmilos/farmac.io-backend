namespace Farmacio_Models.Domain
{
    public class Pharmacist : MedicalStaff
    {
        public virtual Pharmacy Pharmacy { get; set; }
        public virtual WorkTime WorkTime { get; set; }
    }
}
