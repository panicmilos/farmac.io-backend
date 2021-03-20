using EmailService.Constracts;
using EmailService.Implementation;
using EmailService.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmailService.Extensions
{
    public static class AddTemplateProviderExtension
    {
        public static void AddTemplateProvider(this IServiceCollection services, Action<ITemplateProviderOptionsBuilder> buildingActions)
        {
            var options = GetOptions(buildingActions);
            var textOptionsProvider = new TemplatesProvider();

            AddTemplatesFromAssemblyToTemplateProvider<Email>(options.AssemblyType, textOptionsProvider);
            AddTemplatesFromAssemblyToTemplateProvider<EmailImage>(options.AssemblyType, textOptionsProvider);
            AddTemplatesFromAssemblyToTemplateProvider<EmailAttachment>(options.AssemblyType, textOptionsProvider);
            AddTemplatesFromAssemblyToTemplateProvider<TextOptions>(options.AssemblyType, textOptionsProvider);

            AddTemplatesFromFilesToTemplateProvider(options.FilesWithTemplates, textOptionsProvider);

            services.AddSingleton(typeof(ITemplatesProvider), textOptionsProvider);
        }

        private static TemplateProviderOptions GetOptions(Action<ITemplateProviderOptionsBuilder> buildingActions)
        {
            var optionsBuilder = new TemplateProviderOptionsBuilder();
            buildingActions(optionsBuilder);

            return optionsBuilder.Build();
        }

        private static void AddTemplatesFromAssemblyToTemplateProvider<T>(Type assemblyType, TemplatesProvider textOptionsProvider)
        {
            assemblyType.Assembly
               .ExportedTypes
               .Where(type => typeof(ITemplate<T>).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
               .Select(Activator.CreateInstance)
               .Cast<ITemplate<T>>()
               .ToList()
               .ForEach(textOptionsTemplate => textOptionsProvider.AddTemplate(textOptionsTemplate.Name, textOptionsTemplate.GetType()));
        }

        private static void AddTemplatesFromFilesToTemplateProvider(IList<string> filesWithTemplates, TemplatesProvider textOptionsProvider)
        {
            foreach (var file in filesWithTemplates)
            {
                var jsonOfTempates = JObject.Parse(File.ReadAllText(file));
                var templates = jsonOfTempates["templates"] as JArray;
                templates.ToList().ForEach(template => textOptionsProvider.AddTemplate(template["name"].ToString(), template["template"].ToString()));
            }
        }
    }
}