using Farmacio_API.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Farmacio_API.Installers
{
    public class FluentValidationInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public FluentValidationInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            _services.AddControllers().AddFluentValidation(settings => settings.RegisterValidatorsFromAssemblyContaining<Startup>());
            _services.AddControllers(options => options.Filters.Add<ValidationFilter>());
        }
    }
}