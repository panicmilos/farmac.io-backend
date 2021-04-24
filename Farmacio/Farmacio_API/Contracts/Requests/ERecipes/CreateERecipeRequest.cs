using System;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.ERecipes
{
    public class CreateERecipeRequest
    {
        public string UniqueId { get; set; }
        public DateTime IssuingDate { get; set; }
        public Guid PatientId { get; set; }
        public virtual List<CreateERecipeMedicineRequest> Medicines { get; set; }
    }
}
