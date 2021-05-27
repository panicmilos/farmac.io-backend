using System;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.PharmacyPriceLists
{
    public class UpdatePharmacyPriceListRequest
    {
        public Guid Id { get; set; }
        public Guid PharmacyId { get; set; }
        public float ExaminationPrice { get; set; }
        public float ConsultationPrice { get; set; }
        public virtual List<CreateMedicinePriceRequest> MedicinePriceList { get; set; }
    }
}