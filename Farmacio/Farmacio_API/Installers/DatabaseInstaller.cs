using Farmacio_API.Settings;
using Farmacio_Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public DatabaseInstaller(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Install()
        {
            var databaseSettings = new DatabaseSettings();
            _configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            _services.AddDbContext<DatabaseContext>(opts =>
                opts.UseLazyLoadingProxies()
                    .UseMySql(databaseSettings.GetConnectionString(),
                        b => b.MigrationsAssembly("Farmacio_API")));
        }
    }
}