using Farmacio_API.Contracts.Requests.SupplierOffers;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreateSupplierOfferRequestExample : IExamplesProvider<CreateSupplierOfferRequest>
    {
        public CreateSupplierOfferRequest GetExamples()
        {
            return new CreateSupplierOfferRequest
            {
                SupplierId = new Guid("08d8fac7-3089-407b-8ef9-4b554714b1e3"),
                PharmacyOrderId = new Guid("08d90442-dd64-40eb-846b-85fa1714b897"),
                TotalPrice = 7000f,
                DeliveryDeadline = DateTime.Now.AddHours(2)
            };
        }
    }
}