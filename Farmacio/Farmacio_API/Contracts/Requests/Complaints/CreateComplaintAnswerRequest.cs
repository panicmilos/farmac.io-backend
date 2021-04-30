using System;

namespace Farmacio_API.Contracts.Requests.Complaints
{
    public class CreateComplaintAnswerRequest
    {
        public string Text { get; set; }
        public Guid WriterId { get; set; }
        public Guid ComplaintId { get; set; }
    }
}