namespace Farmacio_Models.DTO
{
    public class CheckMedicineDTO : MedicineInPharmacyDTO
    {
        public bool IsAllergy { get; set; }
        public bool IsReplacement { get; set; }
    }
}
