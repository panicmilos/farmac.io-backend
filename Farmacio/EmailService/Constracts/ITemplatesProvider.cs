using System;

namespace EmailService.Constracts
{
    public interface ITemplatesProvider<T>
    {
        void AddTemplate(string templateName, Type templateType);

        T FromTemplate(string templateName, params object[] templateParams);
    }
}