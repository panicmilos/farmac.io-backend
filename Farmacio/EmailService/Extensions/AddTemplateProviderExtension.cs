using EmailService.Constracts;
using EmailService.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EmailService.Extensions
{
    public static class AddTemplateProviderExtension
    {
        public static void AddTemplateProvider<T>(this IServiceCollection services, Type assemblyType)
        {
            var textOptionsProvider = new TemplatesProvider<T>();

            assemblyType.Assembly
                        .ExportedTypes
                        .Where(type => typeof(ITemplate<T>).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        .Select(Activator.CreateInstance)
                        .Cast<ITemplate<T>>()
                        .ToList()
                        .ForEach(textOptionsTemplate => textOptionsProvider.AddTemplate(textOptionsTemplate.Name, textOptionsTemplate.GetType()));

            services.AddSingleton(typeof(ITemplatesProvider<T>), textOptionsProvider);
        }
    }
}