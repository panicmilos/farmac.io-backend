using EmailService.Extensions;
using Farmacio_API.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class EmailServiceInstaller : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public EmailServiceInstaller(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Install()
        {
            var emailServiceSettings = new EmailServiceSettings();
            _configuration.GetSection(nameof(EmailServiceSettings)).Bind(emailServiceSettings);

            _services.AddEmailDispatcher(options =>
            {
                options.AddServer(emailServiceSettings.Host, emailServiceSettings.Port)
                       .AddCredentials(emailServiceSettings.Username, emailServiceSettings.Password)
                       .EnableSsl();
            });

            _services.AddTemplateProvider(options =>
            {
                options.SetAssemblyType(typeof(Startup))
                       .AddFileWithTemplates("templates.json");
            });
        }
    }
}