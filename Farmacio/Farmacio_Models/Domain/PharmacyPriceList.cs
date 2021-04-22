using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class PharmacyPriceList : BaseEntity
    {
        public Guid PharmacyId { get; set; }
        public float ExaminationPrice { get; set; }
        public float ConsultationPrice { get; set; }
        public virtual List<MedicinePrice> MedicinePriceList { get; set; }
    }

    public class MedicinePrice : BaseEntity
    {
        public Guid PharmacyPriceListId { get; set; }
        public float Price { get; set; }
        public DateTime ActiveFrom { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}