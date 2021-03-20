using EmailService.Constracts;
using EmailService.Contracts;
using EmailService.Enums;
using EmailService.Models;
using System.Collections.Generic;

namespace EmailService.Implementation
{
    public class HtmlEmailBuilder : IEmailBuilder
    {
        private string _subject;
        private string _from;
        private readonly List<string> _recipients;
        private readonly IBodyBuilder _bodyBuilder;
        private readonly Dictionary<string, AttachmentType> _attachments;

        public HtmlEmailBuilder()
        {
            _recipients = new List<string>();
            _bodyBuilder = new HttpBodyBuilder();
            _attachments = new Dictionary<string, AttachmentType>();
        }

        public IEmailBuilder AddSubject(string subject)
        {
            _subject = subject;

            return this;
        }

        public IEmailBuilder AddFrom(string emailAddress)
        {
            _from = emailAddress;

            return this;
        }

        public IEmailBuilder AddTo(string emailAddress)
        {
            _recipients.Add(emailAddress);

            return this;
        }

        public IEmailBuilder AddTo(List<string> emailAddresses)
        {
            _recipients.AddRange(emailAddresses);

            return this;
        }

        public IBodyBuilder AddBody()
        {
            return _bodyBuilder;
        }

        public IEmailBuilder AddAttachment(string attachmentPath, AttachmentType type)
        {
            _attachments.Add(attachmentPath, type);

            return this;
        }

        public IEmailBuilder AddAttachment(EmailAttachment attachment)
        {
            return AddAttachment(attachment.Source, attachment.Type);
        }

        public Email Build()
        {
            return new Email
            {
                Subject = _subject,
                From = _from,
                Recipients = _recipients,
                Body = _bodyBuilder.Build(),
                Attachments = _attachments
            };
        }
    }
}