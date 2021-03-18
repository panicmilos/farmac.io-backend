using System;
using System.Collections.Generic;

namespace EmailService.Models
{
    public class TemplateProviderOptions
    {
        public Type AssemblyType { get; set; }
        public IList<string> FilesWithTemplates { get; set; }

        public TemplateProviderOptions()
        {
            FilesWithTemplates = new List<string>();
        }
    }
}