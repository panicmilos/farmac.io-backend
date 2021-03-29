using System;
using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Pharmacy : BaseEntity
    {
        public string Name { get; set; }
        public virtual Address Address { get; set; }
        public string Description { get; set; }
        public virtual List<Pharmacist> Pharmacists { get; set; }
        public virtual List<Dermatologist> Dermatologists { get; set; }
        public virtual PharmacyPriceList PriceList { get; set; }
        public virtual List<PharmacyMedicine> Stock { get; set; }
        public virtual List<PharmacyOrder> Orders { get; set; }
        public virtual List<Promotion> Promotions { get; set; }
        public int AverageGrade { get; set; }
        public virtual List<Grade> Grades { get; set; }
    }

    public class PharmacyMedicine : BaseEntity
    {
        public Guid MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }
}