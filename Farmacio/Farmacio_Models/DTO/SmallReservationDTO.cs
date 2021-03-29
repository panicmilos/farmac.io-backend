using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class SmallReservationDTO
    {
        public Guid ReservationId { get; set; }
        public DateTime PickupDeadline { get; set; }
    }
}
