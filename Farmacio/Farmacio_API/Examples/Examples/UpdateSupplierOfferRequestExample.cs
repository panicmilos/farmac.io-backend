using Farmacio_API.Contracts.Requests.SupplierOffers;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateSupplierOfferRequestExample : IExamplesProvider<UpdateSupplierOfferRequest>
    {
        public UpdateSupplierOfferRequest GetExamples()
        {
            return new UpdateSupplierOfferRequest
            {
                Id = new Guid("08d9044c-0abc-4671-81a4-fa0bc099a0fb"),
                TotalPrice = 9000f,
                DeliveryDeadline = DateTime.Now.AddHours(5)
            };
        }
    }
}