using EmailService.Enums;
using System.Collections.Generic;

namespace EmailService.Utils
{
    public static class AttachmentsToMediaTypeMapper
    {
        private static readonly IDictionary<AttachmentType, string> _mappingPool = new Dictionary<AttachmentType, string>()
        {
            { AttachmentType.PlainText, "text/plain" },
            { AttachmentType.RichText, "text/richtext" },
            { AttachmentType.Html, "text/html" },
            { AttachmentType.Jpeg, "image/jpeg" },
            { AttachmentType.Pdf, "application/pdf" },
            { AttachmentType.Zip, "application/zip" }
        };

        public static string Map(AttachmentType type)
        {
            return _mappingPool[type];
        }
    }
}