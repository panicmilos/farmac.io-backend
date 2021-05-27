namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class UpdateMedicineIngredientRequest
    {
        public string Name { get; set; }
        public float MassInMilligrams { get; set; }
    }
}