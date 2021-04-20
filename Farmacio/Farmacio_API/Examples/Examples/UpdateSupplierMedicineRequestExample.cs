using Farmacio_API.Contracts.Requests.SupplierMedicines;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateSupplierMedicineRequestExample : IExamplesProvider<UpdateSupplierMedicineRequest>
    {
        public UpdateSupplierMedicineRequest GetExamples()
        {
            return new UpdateSupplierMedicineRequest
            {
                Id = new Guid("08d90416-1294-4b08-8d0e-d8710dcc6c86"),
                Quantity = 50
            };
        }
    }
}