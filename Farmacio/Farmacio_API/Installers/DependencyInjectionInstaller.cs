﻿using Farmacio_Repositories.Contracts;
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
            _services.AddScoped(typeof(IAccountService), typeof(AccountService));
            _services.AddScoped(typeof(IPatientService), typeof(PatientService));
            _services.AddScoped(typeof(IPharmacyService), typeof(PharmacyService));
            _services.AddScoped(typeof(IPharmacistService), typeof(PharmacistService));
            _services.AddScoped(typeof(IDermatologistService), typeof(DermatologistService));
            _services.AddScoped(typeof(IPharmacyAdminService), typeof(PharmacyAdminService));
            _services.AddScoped(typeof(IMedicineService), typeof(MedicineService));
            _services.AddScoped(typeof(IReservationService), typeof(ReservationService));
            _services.AddScoped(typeof(IDermatologistService), typeof(DermatologistService));
            _services.AddScoped(typeof(IPharmacyAdminService), typeof(PharmacyAdminService));
            _services.AddScoped(typeof(ISystemAdminService), typeof(SystemAdminService));
            _services.AddScoped(typeof(IEmailVerificationService), typeof(EmailVerificationService));
            _services.AddScoped(typeof(ITokenProvider), typeof(TokenProvider));
            _services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));

            _services.AddScoped<IDummyService, DummyService>();
            _services.AddScoped(typeof(IWeatherForecastService), typeof(WeatherForecastService));
        }
    }
}