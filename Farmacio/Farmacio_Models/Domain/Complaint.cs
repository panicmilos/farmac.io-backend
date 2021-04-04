using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Complaint : BaseEntity
    {
        public Guid WriterId { get; set; }
        public virtual Patient Writer { get; set; }
        public string Text { get; set; }
        public bool IsAboutPharmacy { get; set; }
        public virtual List<ComplaintAnswer> Answers { get; set; }
    }

    public class ComplaintAnswer : BaseEntity
    {
        public string Text { get; set; }
        public Guid WriterId { get; set; }
        public virtual SystemAdmin Writer { get; set; }
        public Guid ComplaintId { get; set; }
    }

    public class ComplaintAboutPharmacy : Complaint
    {
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
    
    public class ComplaintAboutStaff : Complaint
    {
        public Guid StaffId { get; set; }
        public virtual MedicalStaff Staff { get; set; }
    }
}

