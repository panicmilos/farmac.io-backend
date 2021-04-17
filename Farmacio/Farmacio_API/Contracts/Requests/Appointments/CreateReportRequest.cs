namespace Farmacio_API.Contracts.Requests.Appointments
{
    public class CreateReportRequest
    {
        public string Notes { get; set; }
        public int TherapyDurationInDays { get; set; }
    }
}
