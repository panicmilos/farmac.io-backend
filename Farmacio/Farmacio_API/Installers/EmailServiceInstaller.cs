using EmailService.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class EmailServiceInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public EmailServiceInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddEmailDispatcher(options =>
            {
                options.AddServer("smtp-relay.sendinblue.com", 587)
                       .AddCredentials("panic.milos99@gmail.com", "YRphJz8FQ0LwBImM")
                       .EnableSsl();
            });
        }
    }
}