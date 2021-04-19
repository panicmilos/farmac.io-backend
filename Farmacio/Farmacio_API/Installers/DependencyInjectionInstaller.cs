using Farmacio_Repositories.Contracts;
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
            _services.AddScoped(typeof(ISupplierService), typeof(SupplierService));
            _services.AddScoped(typeof(IAppointmentService), typeof(AppointmentService));
            _services.AddScoped(typeof(IMedicineService), typeof(MedicineService));
            _services.AddScoped(typeof(IMedicineReplacementService), typeof(MedicineReplacementService));
            _services.AddScoped(typeof(IMedicineIngredientService), typeof(MedicineIngredientService));
            _services.AddScoped(typeof(IReservationService), typeof(ReservationService));
            _services.AddScoped(typeof(IDermatologistService), typeof(DermatologistService));
            _services.AddScoped(typeof(IPharmacyAdminService), typeof(PharmacyAdminService));
            _services.AddScoped(typeof(ISystemAdminService), typeof(SystemAdminService));
            _services.AddScoped(typeof(IEmailVerificationService), typeof(EmailVerificationService));
            _services.AddScoped(typeof(ITokenProvider), typeof(TokenProvider));
            _services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
            _services.AddScoped(typeof(IDermatologistWorkPlaceService), typeof(DermatologistWorkPlaceService));
            _services.AddScoped(typeof(IPharmacyPriceListService), typeof(PharmacyPriceListService));
            _services.AddScoped(typeof(IPharmacyStockService), typeof(PharmacyStockService));
            _services.AddScoped(typeof(IMedicalStaffService), typeof(MedicalStaffService));
            _services.AddScoped(typeof(IReportService), typeof(ReportService));
            _services.AddScoped(typeof(IPatientAllergyService), typeof(PatientAllergyService));
            _services.AddScoped(typeof(IMedicinePdfService), typeof(MedicinePdfService));
        }
    }
}