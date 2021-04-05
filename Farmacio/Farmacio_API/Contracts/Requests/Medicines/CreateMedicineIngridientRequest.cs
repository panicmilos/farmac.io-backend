namespace Farmacio_API.Contracts.Requests.Medicines
{
    public class CreateMedicineIngridientRequest
    {
        public string Name { get; set; }
        public float MassInMilligrams { get; set; }
    }
}