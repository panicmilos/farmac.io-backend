using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class PharmacySearchParams
    {
        public string Name { get; set; }
        public string StreetAndCity { get; set; }

        public void Deconstruct(out string name, out string streetAndCity)
        {
            name = Name;
            streetAndCity = StreetAndCity;
        }
    }
}
