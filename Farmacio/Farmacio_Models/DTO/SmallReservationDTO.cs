using System;

namespace Farmacio_Models.DTO
{
    public class SmallReservationDTO
    {
        public Guid ReservationId { get; set; }
        public string UniqueId { get; set; }
        public DateTime PickupDeadline { get; set; }
        public Guid PharmacyId { get; set; }
        public float Price { get; set; }
    }
}
