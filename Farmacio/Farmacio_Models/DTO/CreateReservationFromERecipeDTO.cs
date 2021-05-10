using System;

namespace Farmacio_Models.DTO
{
    public class CreateReservationFromERecipeDTO
    {
        public Guid ERecipeId { get; set; }
        public Guid PharmacyId { get; set; }
        public DateTime PickupDeadline { get; set; }
    }
}