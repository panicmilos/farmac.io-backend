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
        public int GradeFrom { get; set; }
        public int GradeTo { get; set; }
        public double DistanceFrom { get; set; }
        public double DistanceTo { get; set; }
        public float UserLat { get; set; }
        public float UserLon { get; set; }

        public void Deconstruct(out string name, out string streetAndCity, out string sortCriteria, out bool isAscending, out int gradeFrom,
            out int gradeTo, out double distanceFrom, out double distanceTo, out float userLat, out float userLon)
        {
            name = Name;
            streetAndCity = StreetAndCity;
            sortCriteria = SortCriteria;
            isAscending = IsAscending;
            gradeFrom = GradeFrom;
            gradeTo = GradeTo;
            distanceFrom = DistanceFrom;
            distanceTo = DistanceTo;
            userLat = UserLat;
            userLon = UserLon;
        }
    }
}
