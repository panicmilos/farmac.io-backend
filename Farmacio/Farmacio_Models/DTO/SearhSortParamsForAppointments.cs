using System;

namespace Farmacio_Models.DTO
{
    public class SearhSortParamsForAppointments
    {
        public DateTime ConsultationDateTime { get; set; }
        public int Duration { get; set; }
        public String SortCriteria { get; set; }
        public bool IsAsc { get; set; }
    }
}
