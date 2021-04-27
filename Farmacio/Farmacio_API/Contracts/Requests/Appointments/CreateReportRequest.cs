using Farmacio_API.Contracts.Requests.ERecipes;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.Appointments
{
    public class CreateReportRequest
    {
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
        public virtual List<CreateERecipeMedicineRequest> PrescribedMedicines { get; set; }
    }
}
