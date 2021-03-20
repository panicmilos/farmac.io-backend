using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Reservation : BaseEntity
    {
        public string UniqueId { get; set; }
        public ReservationState State { get; set; }
        public DateTime PickupDeadline { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public Patient Patient { get; set; }
        public List<ReservedMedicine> Medicines { get; set; }
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
        public Medicine Medicine { get; set; }
    }
}
