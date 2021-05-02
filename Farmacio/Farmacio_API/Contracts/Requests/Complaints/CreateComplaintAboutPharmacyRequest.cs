using System;

namespace Farmacio_API.Contracts.Requests.Complaints
{
    public class CreateComplaintAboutPharmacyRequest
    {
        public Guid WriterId { get; set; }
        public string Text { get; set; }
        public Guid PharmacyId { get; set; }
    }
}