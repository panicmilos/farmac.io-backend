using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Pharmacy : BaseEntity, IGradeable
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public List<Pharmacist> Pharmacists { get; set; }
        public List<Dermatologist> Dermatologists { get; set; }
        public PharmacyPriceList PriceList { get; set; }
        public List<PharmacyOrder> Orders { get; set; }
        public int Grade { get; set; }
    }
}
