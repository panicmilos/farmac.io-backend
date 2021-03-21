using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Medicine : BaseEntity
    {
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public MedicineForm Form { get; set; }
        public virtual MedicineType Type { get; set; }
        public string Manufacturer { get; set; }
        public bool IsRecipeOnly { get; set; }
        public string Contraindications { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecommendedDose { get; set; }
        public virtual List<MedicineIngredient> MedicineIngredients { get; set; }
        public virtual List<Medicine> Replacements { get; set; }
        public int AverageGrade { get; set; }
        public virtual List<Grade> Grades { get; set; }
    }

    public enum MedicineForm
    {
        Powder,
        Capsule,
        Tablet,
        Mast,
        Pasta,
        Gel,
        Syrup,
        Solution,
        Other
    }

    public class MedicineType : BaseEntity
    {
        public string TypeName { get; set; }
    }

    public class MedicineIngredient : BaseEntity
    {
        public virtual Ingredient Ingredient { get; set; }
        public float MassInMilligramms { get; set; }
    }
}
