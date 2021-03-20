using EmailService.Constracts;
using Newtonsoft.Json.Linq;
using Scriban;
using System;
using System.Collections.Generic;

namespace EmailService.Implementation
{
    public class TemplatesProvider : ITemplatesProvider
    {
        private readonly IDictionary<string, Type> _typeTemplates;
        private readonly IDictionary<string, string> _stringTemplates;

        public TemplatesProvider()
        {
            _typeTemplates = new Dictionary<string, Type>();
            _stringTemplates = new Dictionary<string, string>();
        }

        public void AddTemplate(string templateName, Type templateType)
        {
            _typeTemplates.Add(templateName, templateType);
        }

        public void AddTemplate(string templateName, string template)
        {
            _stringTemplates.Add(templateName, template);
        }

        public T FromTemplate<T>(string templateName, object templateParams = null)
        {
            var stringTemplate = "";

            if (_typeTemplates.TryGetValue(templateName, out var templateType))
            {
                var templateGenerator = Activator.CreateInstance(templateType) as ITemplate<T>;
                var templateObject = templateGenerator.GetTemplate();
                stringTemplate = JObject.FromObject(templateObject).ToString();
            }

            if (_stringTemplates.TryGetValue(templateName, out var template))
            {
                stringTemplate = template;
            }

            var scribamTemplate = Template.Parse(stringTemplate);
            var result = scribamTemplate.Render(templateParams);
            var jsonOfTemplate = JObject.Parse(result);

            return jsonOfTemplate.ToObject<T>();
        }
    }
}