using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Reservation : BaseEntity
    {
        public string UniqueId { get; set; }
        public ReservationState State { get; set; }
        public DateTime PickupDeadline { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual List<ReservedMedicine> Medicines { get; set; }
    }

    public enum ReservationState
    {
        Reserved,
        Done,
        Cancelled
    }

    public class ReservedMedicine : BaseEntity
    {
        public float Price { get; set; }
        public int Quantity { get; set; }
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}