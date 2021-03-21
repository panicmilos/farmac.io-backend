namespace Farmacio_Models.Domain
{
    public class PharmacyAdmin : User
    {
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
