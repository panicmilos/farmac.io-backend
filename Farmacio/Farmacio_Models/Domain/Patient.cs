using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Patient : User
    {
        public int Points { get; set; }
        public int NegativePoints { get; set; }

        //public Guid LoyaltyProgramId { get; set; }
        public virtual LoyaltyProgram LoyaltyProgram { get; set; }
    }

    public class PatientPharmacyFollow : BaseEntity
    {
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }

    public class PatientAllergy : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}