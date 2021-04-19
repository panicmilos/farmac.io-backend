using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Implementation.Utils
{
    public static class LatLngUtils
    {
        /// <summary>
        /// This is Haversine formula.
        /// Link to formula: http://www.movable-type.co.uk/scripts/latlong.html 
        /// </summary>
        public static double GetDistanceFromLatLngInKm(CoordinatesDTO from, CoordinatesDTO to)
        {
            var earthRadius = 6371;
            var dLat = DegToRad(to.Lat - from.Lat);
            var dLng = DegToRad(to.Lng - from.Lng);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegToRad(from.Lat)) * Math.Cos(DegToRad(to.Lat)) *
                    Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = earthRadius * c;
            return d;
        }


        private static double DegToRad(float deg)
        {
            return deg * (Math.PI / 180);
        }

        public static bool IsLngValid(float lng)
        {
            return lng >= -180 && lng <= 180;
        }

        public static bool IsLatValid(float lat)
        {
            return lat >= -90 && lat <= 90;
        }
    }
}
