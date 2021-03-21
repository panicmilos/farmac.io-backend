using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Complaint : BaseEntity
    {
        public virtual Patient Writer { get; set; }
        public string Text { get; set; }
        public bool IsAboutPharmacy { get; set; }
        public virtual List<ComplaintAnswer> Answers { get; set; }
    }

    public class ComplaintAnswer : BaseEntity
    {
        public string Text { get; set; }
        public virtual SystemAdmin Writer { get; set; }
        public virtual Complaint Complaint { get; set; }
    }

    public class ComplaintAboutPharmacy : Complaint
    {
        public virtual Pharmacy Pharmacy { get; set; }
    }
    
    public class ComplaintAboutStaff : Complaint
    {
        public virtual MedicalStaff Staff { get; set; }
    }
}

