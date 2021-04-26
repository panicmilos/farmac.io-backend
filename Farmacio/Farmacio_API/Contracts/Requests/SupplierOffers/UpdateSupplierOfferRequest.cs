﻿using System;

namespace Farmacio_API.Contracts.Requests.SupplierOffers
{
    public class UpdateSupplierOfferRequest
    {
        public Guid Id { get; set; }
        public DateTime DeliveryDeadline { get; set; }
        public float TotalPrice { get; set; }
    }
}