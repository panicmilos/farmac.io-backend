using System;
using System.Collections.Generic;

namespace Farmacio_API.Contracts.Requests.Reservations
{
    public class CreateReservationRequest
    {
        public DateTime PickupDeadline { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid PatientId { get; set; }
        public virtual List<CreateReservedMedicineRequest> Medicines { get; set; }
    }
}