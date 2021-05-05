using System;
using System.Collections.Generic;
using System.Text;

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
