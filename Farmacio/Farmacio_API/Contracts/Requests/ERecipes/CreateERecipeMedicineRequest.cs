using System;

namespace Farmacio_API.Contracts.Requests.ERecipes
{
    public class CreateERecipeMedicineRequest
    {
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
