using System;
using Farmacio_Models.Domain;
using Farmacio_Services.Exceptions;

namespace Farmacio_Services.Implementation.Validation
{
    public static class WorkTimeValidation
    {
        public static void ValidateWorkHours(WorkTime workTime)
        {
            var workTimeHourDiff = Math.Abs(workTime.From.Hour - workTime.To.Hour);
            var workTimeMinuteDiff = Math.Abs(workTime.From.Minute - workTime.To.Minute);
            if (workTimeHourDiff < 1 || workTimeHourDiff > 8 || (workTimeHourDiff == 8 && workTimeMinuteDiff != 0))
                throw new InvalidWorkTimeException("Work time must be minimum 1 hour and maximum 8 hours long.");
        }
    }
}