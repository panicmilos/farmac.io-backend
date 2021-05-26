using EmailService.Contracts;
using EmailService.Models;
using System;

namespace EmailService.Constracts
{
    public interface ITemplateProviderOptionsBuilder : IBuilder<TemplateProviderOptions>
    {
        ITemplateProviderOptionsBuilder SetAssemblyType(Type assemblyType);

        ITemplateProviderOptionsBuilder AddFileWithTemplates(string filePath);
    }
}