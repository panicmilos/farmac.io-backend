namespace Farmacio_Models.Domain
{
    public class Complaint : BaseEntity
    {
        public Patient Writer { get; set; }
        public string Text { get; set; }
    }

    public class ComplaintAnswer : BaseEntity
    {
        public string Text { get; set; }
        public SystemAdmin Writer { get; set; }
        public Complaint Complaint { get; set; }
    }

    public class ComplaintAboutPharmacy : Complaint
    {
        public Pharmacy Pharmacy { get; set; }
    }
    
    public class ComplaintAboutStaff : Complaint
    {
        public MedicalStaff Staff { get; set; }
    }
}

