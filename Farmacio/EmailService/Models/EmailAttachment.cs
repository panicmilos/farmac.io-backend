using EmailService.Enums;

namespace EmailService.Models
{
    public class EmailAttachment
    {
        public string Source { get; set; }
        public AttachmentType Type { get; set; }
    }
}