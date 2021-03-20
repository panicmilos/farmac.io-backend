using System;

namespace Farmacio_Models.Domain
{
    public class SupplierOffer : BaseEntity
    {
        public Supplier Supplier { get; set; }
        public DateTime DeliveryDeadline { get; set; }
        public float TotalPrice { get; set; }
        public OfferStatus Status { get; set; }
        public PharmacyOrder PharmacyOrder { get; set; }
    }

    public enum OfferStatus
    {
        Accepted,
        Refused,
        WaitingForAnswer
    }
}
