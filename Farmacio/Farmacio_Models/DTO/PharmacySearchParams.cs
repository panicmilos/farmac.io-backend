using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class PharmacySearchParams
    {
        public string Name { get; set; }
        public string StreetAndCity { get; set; }
        public string SortCriteria { get; set; }
        public bool IsAscending { get; set; }

        public void Deconstruct(out string name, out string streetAndCity, out string sortCriteria, out bool isAscending)
        {
            name = Name;
            streetAndCity = StreetAndCity;
            sortCriteria = SortCriteria;
            isAscending = IsAscending;
        }
    }
}
