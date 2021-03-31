using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class LoyaltyPoints : BaseEntity
    {
        public int ConsultationPoints { get; set; }
        public int ExaminationPoints { get; set; }
        public virtual List<MedicinePoints> MedicinePointsList { get; set; }
    }

    public class MedicinePoints : BaseEntity
    {
        public Guid MedicineId { get; set; }
        public int Points { get; set; }
    }
}
