using EmailService.Constracts;
using EmailService.Implementation;
using EmailService.Models;
using Farmacio_API.Templates.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacio_API.Templates.Emails
{
    public class ExampleEmail1 : ITemplate<Email>
    {
        public string Name { get; } = "Email1";

        public Email GetTemplate(params object[] templateParams)
        {
            var emailBuilder = new HtmlEmailBuilder();

            emailBuilder
                .AddSubject("Testiram web klijent")
                .AddFrom("panic.milos99@gmail.com")
                .AddTo(templateParams[0].ToString())
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\semasenzora.jpg", AttachmentType.Jpeg)
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\testzip.zip", AttachmentType.Zip)
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\vju.txt", AttachmentType.PlainText)
                .AddBody()
                .AddText($"Postovani {templateParams[1]},", new ExampleFont2().GetTemplate())
                .AddImageFromUrl("https://img.cdn-cnj.si/img/250/250/6U/6UriUZmyd0t")
                .AddNewLine()
                .AddUnorderedList(new List<string>() { "Pas", "Na Svetu" });

            return emailBuilder.Build();
        }
    }
}