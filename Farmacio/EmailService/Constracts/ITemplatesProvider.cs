using System;

namespace EmailService.Constracts
{
    public interface ITemplatesProvider
    {
        void AddTemplate(string templateName, Type templateType);

        void AddTemplate(string templateName, string template);

        T FromTemplate<T>(string templateName, object templateParams = null);
    }
}