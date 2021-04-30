using System;

namespace Farmacio_Models.DTO
{
    public class AppointmentAsEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }   // patient name
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string PharmacyName { get; set; }
        public bool IsReported { get; set; }
    }
}
