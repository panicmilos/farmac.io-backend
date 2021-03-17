using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class AutoMapperInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public AutoMapperInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddAutoMapper(typeof(Startup));
        }
    }
}