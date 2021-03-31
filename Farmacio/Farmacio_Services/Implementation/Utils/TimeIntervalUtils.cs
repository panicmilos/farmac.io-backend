using System;

namespace Farmacio_Services.Implementation.Utils
{
    public static class TimeIntervalUtils
    {
        public static bool TimeIntervalTimesOverlap(DateTime firstFrom, DateTime firstTo, DateTime secondFrom, DateTime secondTo)
        {
            var hoursFrom = firstFrom.Hour;
            var minutesFrom = firstFrom.Minute;
            var hoursTo = firstTo.Hour;
            var minutesTo = firstTo.Minute;

            var isBefore = secondTo.Hour < hoursFrom ||
                           (secondTo.Hour == hoursFrom && secondTo.Minute <= minutesFrom);

            var isAfter = secondFrom.Hour > hoursTo ||
                          (secondFrom.Hour == hoursTo && secondFrom.Minute >= minutesTo);

            return !(isBefore || isAfter);
        }
    }
}