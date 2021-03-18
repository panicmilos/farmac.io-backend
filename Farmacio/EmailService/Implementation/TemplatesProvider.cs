using EmailService.Constracts;
using System;
using System.Collections.Generic;

namespace EmailService.Implementation
{
    public class TemplatesProvider<T> : ITemplatesProvider<T>
    {
        private IDictionary<string, Type> _templates;

        public TemplatesProvider()
        {
            _templates = new Dictionary<string, Type>();
        }

        public void AddTemplate(string templateName, Type templateType)
        {
            _templates.Add(templateName, templateType);
        }

        public T FromTemplate(string templateName, params object[] templateParams)
        {
            var templateType = _templates[templateName];
            var template = Activator.CreateInstance(templateType) as ITemplate<T>;

            return template.GetTemplate(templateParams);
        }
    }
}