using System;

namespace Farmacio_API.Contracts.Requests.Complaints
{
    public class CreateComplaintAboutPharmacistRequest
    {
        public Guid WriterId { get; set; }
        public string Text { get; set; }
        public Guid PharmacistId { get; set; }
    }
}