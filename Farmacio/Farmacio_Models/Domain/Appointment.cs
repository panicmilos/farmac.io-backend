using System;

namespace Farmacio_Models.Domain
{
    public class Appointment : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public bool IsReserved { get; set; }
        public Guid? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public Guid MedicalStaffId { get; set; }
        public virtual MedicalStaff MedicalStaff { get; set; }
        public Guid? ReportId { get; set; }
        public virtual Report Report { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
