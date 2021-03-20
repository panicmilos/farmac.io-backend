using EmailService.Constracts;
using EmailService.Implementation;
using EmailService.Models;
using Farmacio_API.Templates.Fonts;
using System.Collections.Generic;

namespace Farmacio_API.Templates.Emails
{
    public class ExampleEmail1 : ITemplate<Email>
    {
        public string Name { get; } = "Email1";

        public Email GetTemplate()
        {
            var emailBuilder = new HtmlEmailBuilder();

            emailBuilder
                .AddSubject("Testiram web klijent")
                .AddFrom("panic.milos99@gmail.com")
                .AddTo("{{to}}")
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\semasenzora.jpg", AttachmentType.Jpeg)
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\testzip.zip", AttachmentType.Zip)
                //.AddAttachment(@"C:\Users\panic\OneDrive\Desktop\vju.txt", AttachmentType.PlainText)
                .AddBody()
                .AddText("Postovani {{name}},", new ExampleFont2().GetTemplate())
                .AddImage("https://img.cdn-cnj.si/img/250/250/6U/6UriUZmyd0t")
                .AddNewLine()
                .AddUnorderedList(new List<string>() { "Pas", "Na Svetu" });

            return emailBuilder.Build();
        }
    }
}