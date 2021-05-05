using System;

namespace Farmacio_API.Contracts.Requests.ERecipes
{
    public class CreateReservationFromERecipeRequest
    {
        public Guid ERecipeId { get; set; }
        public Guid PharmacyId { get; set; }
        public DateTime PickupDeadline { get; set; }
    }
}