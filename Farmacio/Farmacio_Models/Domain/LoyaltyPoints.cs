using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class LoyaltyPoints : BaseEntity
    {
        public int ConsultationPoints { get; set; }
        public int ExaminationPoints { get; set; }
        public List<MedicinePoints> MedicinePointsList { get; set; }
    }

    public class MedicinePoints : BaseEntity
    {
        public Medicine Medicine { get; set; }
        public int Points { get; set; }
    }
}
