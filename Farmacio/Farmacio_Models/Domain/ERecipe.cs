using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class ERecipe : BaseEntity
    {
        public string UniqueId { get; set; }
        public DateTime IssuingDate { get; set; }
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual List<ERecipeMedicine> Medicines { get; set; }
        public bool IsUsed { get; set; }
    }

    public class ERecipeMedicine : BaseEntity
    {
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }
}
