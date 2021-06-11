using System;

namespace Farmacio_Models.DTO
{
    public class WorkCalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }   // patient name or absence type
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string PharmacyName { get; set; }
        public bool IsReported { get; set; }
        public bool IsAppointment { get; set; } // true: Appointment, false: Absence
    }
}
