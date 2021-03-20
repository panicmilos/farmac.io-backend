using System;

namespace Farmacio_Models.Domain
{
    public class Appointment : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public bool IsReserved { get; set; }
        public Patient Patient { get; set; }
        public MedicalStaff MedicalStaff { get; set; }
        public Report Report { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}
