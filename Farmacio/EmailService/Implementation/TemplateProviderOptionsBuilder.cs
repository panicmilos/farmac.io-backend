using EmailService.Constracts;
using EmailService.Models;
using System;
using System.Collections.Generic;

namespace EmailService.Implementation
{
    public class TemplateProviderOptionsBuilder : ITemplateProviderOptionsBuilder
    {
        private Type _assemblyType;
        private readonly IList<string> _filesWithTemplates;

        public TemplateProviderOptionsBuilder()
        {
            _filesWithTemplates = new List<string>();
        }

        public ITemplateProviderOptionsBuilder SetAssemblyType(Type assemblyType)
        {
            _assemblyType = assemblyType;

            return this;
        }

        public ITemplateProviderOptionsBuilder AddFileWithTemplates(string filePath)
        {
            _filesWithTemplates.Add(filePath);

            return this;
        }

        public TemplateProviderOptions Build()
        {
            return new TemplateProviderOptions
            {
                AssemblyType = _assemblyType,
                FilesWithTemplates = _filesWithTemplates
            };
        }
    }
}