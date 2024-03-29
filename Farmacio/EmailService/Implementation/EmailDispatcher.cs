﻿using EmailService.Constracts;
using EmailService.Models;
using EmailService.Utils;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace EmailService.Implementation
{
    public class EmailDispatcher : IEmailDispatcher
    {
        private readonly SmtpClient _sendingClient;

        public EmailDispatcher(SmtpServerOptions options)
        {
            _sendingClient = new SmtpClient()
            {
                Host = options.Host,
                Port = options.Port,
                Credentials = new NetworkCredential(options.Username, options.Password),
                EnableSsl = options.Ssl
            };
        }

        public void Dispatch(Email email)
        {
            var mailMessage = GetMailMessageFromEmail(email);

            _sendingClient.Send(mailMessage);
        }

        private MailMessage GetMailMessageFromEmail(Email email)
        {
            var mailMessage = new MailMessage();

            mailMessage.Subject = email.Subject;
            mailMessage.From = new MailAddress(email.From);
            email.Recipients.ForEach(recipientMail => mailMessage.To.Add(recipientMail));
            mailMessage.Body = email.Body;
            var attachments = email.Attachments.Select(keyValuePair => new Attachment(keyValuePair.Key, AttachmentsToMediaTypeMapper.Map(keyValuePair.Value))).ToList();
            attachments.ForEach(attachment => mailMessage.Attachments.Add(attachment));

            return mailMessage;
        }
    }
}