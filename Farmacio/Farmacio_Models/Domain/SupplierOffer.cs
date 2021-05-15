using System;

namespace Farmacio_Models.Domain
{
    public class SupplierOffer : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public int DeliveryDeadline { get; set; }
        public float TotalPrice { get; set; }
        public OfferStatus Status { get; set; }
        public Guid PharmacyOrderId { get; set; }
        public virtual PharmacyOrder PharmacyOrder { get; set; }
    }

    public enum OfferStatus
    {
        Accepted,
        Refused,
        WaitingForAnswer
    }
}