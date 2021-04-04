using System;

namespace Farmacio_Models.Domain
{
    public class Pharmacist : MedicalStaff
    {
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        public Guid WorkTimeId { get; set; }
        public virtual WorkTime WorkTime { get; set; }
    }
}
