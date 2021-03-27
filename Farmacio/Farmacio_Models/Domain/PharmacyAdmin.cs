using System;

namespace Farmacio_Models.Domain
{
    public class PharmacyAdmin : User
    {
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}