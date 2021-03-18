using EmailService.Enums;
using System.Collections.Generic;

namespace EmailService.Models
{
    public class Email
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public List<string> Recipients { get; set; }
        public string Body { get; set; }
        public Dictionary<string, AttachmentType> Attachments { get; set; }
    }
}