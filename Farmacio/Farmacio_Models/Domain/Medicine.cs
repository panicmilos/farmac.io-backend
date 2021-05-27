using System;
using System.ComponentModel.DataAnnotations;

namespace Farmacio_Models.Domain
{
    public class Medicine : BaseEntity
    {
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public MedicineForm Form { get; set; }
        public Guid TypeId { get; set; }
        public virtual MedicineType Type { get; set; }
        public string Manufacturer { get; set; }
        public bool IsRecipeOnly { get; set; }
        public string Contraindications { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecommendedDose { get; set; }
        [ConcurrencyCheck]
        public float AverageGrade { get; set; }
        public int NumberOfGrades { get; set; }
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
        public Guid MedicineId { get; set; }
        public string Name { get; set; }
        public float MassInMilligrams { get; set; }
    }

    public class MedicineReplacement : BaseEntity
    {
        public Guid MedicineId { get; set; }
        public Guid ReplacementMedicineId { get; set; }
        public virtual Medicine ReplacementMedicine { get; set; }
    }
}