using System;

namespace Farmacio_Models.DTO
{
    public class ERecipeDTO
    {
        public Guid Id { get; set; }
        public string UniqueId {get; set;}
        public DateTime IssuingDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
