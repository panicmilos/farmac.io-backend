using System;

namespace Farmacio_Models.Domain
{
    public class Promotion : BaseEntity
    {
        public int Discount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
