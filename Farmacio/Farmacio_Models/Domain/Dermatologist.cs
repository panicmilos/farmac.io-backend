using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Dermatologist : MedicalStaff
    {
    }

    public class DermatologistWorkPlace : BaseEntity
    {
        public Guid DermatologistId { get; set; }
        public Guid WorkTimeId { get; set; }
        public virtual WorkTime WorkTime { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
