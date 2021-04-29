using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Complaint : BaseEntity
    {
        public Guid WriterId { get; set; }
        public virtual Patient Writer { get; set; }
        public string Text { get; set; }
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

    public class ComplaintAboutDermatologist : Complaint
    {
        public Guid DermatologistId { get; set; }
        public virtual Dermatologist Dermatologist { get; set; }
    }

    public class ComplaintAboutPharmacist : Complaint
    {
        public Guid PharmacistId { get; set; }
        public virtual Pharmacist Pharmacist { get; set; }
    }
}