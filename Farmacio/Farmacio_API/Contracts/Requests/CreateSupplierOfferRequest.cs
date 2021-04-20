using System;

namespace Farmacio_API.Contracts.Requests
{
    public class CreateSupplierOfferRequest
    {
        public Guid SupplierId { get; set; }
        public DateTime DeliveryDeadline { get; set; }
        public float TotalPrice { get; set; }
        public Guid PharmacyOrderId { get; set; }
    }
}