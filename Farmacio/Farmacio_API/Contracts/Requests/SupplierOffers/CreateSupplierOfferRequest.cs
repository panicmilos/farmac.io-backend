using System;

namespace Farmacio_API.Contracts.Requests.SupplierOffers
{
    public class CreateSupplierOfferRequest
    {
        public Guid SupplierId { get; set; }
        public int DeliveryDeadline { get; set; }
        public float TotalPrice { get; set; }
        public Guid PharmacyOrderId { get; set; }
    }
}