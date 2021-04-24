using Farmacio_API.Contracts.Requests.SupplierMedicines;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreateSupplierMedicineRequestExample : IExamplesProvider<CreateSupplierMedicineRequest>
    {
        public CreateSupplierMedicineRequest GetExamples()
        {
            return new CreateSupplierMedicineRequest
            {
                SupplierId = new Guid("08d8fac7-3089-407b-8ef9-4b554714b1e3"),
                MedicineId = new Guid("08d8f514-56dd-42f2-80f3-419aa155282b"),
                Quantity = 100
            };
        }
    }
}