using System;

namespace Farmacio_Models.Domain
{
    public class NotInStock : BaseEntity
    {
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
        public Guid PharmacyId { get; set; }
        public bool IsSeen { get; set; }
    }
}
