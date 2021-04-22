using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;

namespace Farmacio_API.Contracts.Requests.PharmacyOrders
{
    public class CreatePharmacyOrderRequest
    {
        public Guid PharmacyAdminId { get; set; }
        public DateTime OffersDeadline { get; set; }
        public IList<CreatePharmacyMedicineRequest> OrderedMedicines { get; set; }
    }
}