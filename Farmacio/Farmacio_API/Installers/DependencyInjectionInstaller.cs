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
            AddRepositories();
            AddServices();
        }

        private void AddRepositories()
        {
            _services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            _services.AddScoped<IDummyRepository, DummyRepository>();
        }

        private void AddServices()
        {
            _services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
            _services.AddScoped<IDummyService, DummyService>();
            _services.AddScoped(typeof(IWeatherForecastService), typeof(WeatherForecastService));
            _services.AddScoped(typeof(ITokenService), typeof(TokenService));
        }
    }
}