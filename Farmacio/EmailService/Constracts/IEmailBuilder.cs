using EmailService.Constracts;
using EmailService.Enums;
using EmailService.Models;
using System.Collections.Generic;

namespace EmailService.Contracts
{
    public interface IEmailBuilder : IBuilder<Email>
    {
        IEmailBuilder AddSubject(string subject);

        IEmailBuilder AddFrom(string emailAddress);

        IEmailBuilder AddTo(string emailAddress);

        IEmailBuilder AddTo(List<string> emailAddresses);

        IBodyBuilder AddBody();

        IEmailBuilder AddAttachment(string attachmentPath, AttachmentType type);
    }
}