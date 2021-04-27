using System;

namespace Farmacio_API.Contracts.Requests.Complaints
{
    public class CreateComplaintAboutDermatologistRequest
    {
        public Guid WriterId { get; set; }
        public string Text { get; set; }
        public Guid DermatologistId { get; set; }
    }
}