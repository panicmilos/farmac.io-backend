using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.LoyaltyPoints
{
    public class UpdateLoyaltyPointsRequest
    {
        public int ConsultationPoints { get; set; }
        public int ExaminationPoints { get; set; }
        public List<UpdateMedicinePointsRequest> MedicinePointsList { get; set; }
    }
}