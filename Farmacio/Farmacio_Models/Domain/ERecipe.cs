using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class ERecipe : BaseEntity
    {
        public string UniqueId { get; set; }
        public DateTime IssuingDate { get; set; }
        public Patient Patient { get; set; }
        public List<ERecipeMedicine> Medicines { get; set; }
    }

    public class ERecipeMedicine : BaseEntity
    {
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }
}
