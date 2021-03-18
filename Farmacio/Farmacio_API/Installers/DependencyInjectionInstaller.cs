using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class DependencyInjectionInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public DependencyInjectionInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddScoped<IDummyService, DummyService>();
            _services.AddScoped<IDummyRepository, DummyRepository>();
            _services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}