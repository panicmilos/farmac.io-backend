using System;

namespace Farmacio_Models.Domain
{
    public class Appointment : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public bool IsReserved { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual MedicalStaff MedicalStaff { get; set; }
        public virtual Report Report { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
