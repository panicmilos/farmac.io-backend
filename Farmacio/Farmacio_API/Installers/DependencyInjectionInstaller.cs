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
            _services.AddScoped(typeof(IAccountRepository), typeof(AccountRepository));
            _services.AddScoped(typeof(IComplaintAnswerRepository), typeof(ComplaintAnswerRepository));
        }

        private void AddServices()
        {
            _services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
            _services.AddScoped(typeof(IAccountService), typeof(AccountService));
            _services.AddScoped(typeof(IPatientService), typeof(PatientService));
            _services.AddScoped(typeof(IPharmacyService), typeof(PharmacyService));
            _services.AddScoped(typeof(IPharmacyOrderService), typeof(PharmacyOrderService));
            _services.AddScoped(typeof(IPharmacistService), typeof(PharmacistService));
            _services.AddScoped(typeof(IDermatologistService), typeof(DermatologistService));
            _services.AddScoped(typeof(IPharmacyAdminService), typeof(PharmacyAdminService));
            _services.AddScoped(typeof(ISupplierService), typeof(SupplierService));
            _services.AddScoped(typeof(ISupplierStockService), typeof(SupplierStockService));
            _services.AddScoped(typeof(ISupplierOfferService), typeof(SupplierOfferService));
            _services.AddScoped(typeof(IAppointmentService), typeof(AppointmentService));
            _services.AddScoped(typeof(IMedicineService), typeof(MedicineService));
            _services.AddScoped(typeof(IMedicineReplacementService), typeof(MedicineReplacementService));
            _services.AddScoped(typeof(IMedicineIngredientService), typeof(MedicineIngredientService));
            _services.AddScoped(typeof(IReservationService), typeof(ReservationService));
            _services.AddScoped(typeof(IDermatologistService), typeof(DermatologistService));
            _services.AddScoped(typeof(IPharmacyAdminService), typeof(PharmacyAdminService));
            _services.AddScoped(typeof(ISystemAdminService), typeof(SystemAdminService));
            _services.AddScoped(typeof(IPatientFollowingsService), typeof(PatientFollowingsService));
            _services.AddScoped(typeof(IEmailVerificationService), typeof(EmailVerificationService));
            _services.AddScoped(typeof(ITokenProvider), typeof(TokenProvider));
            _services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
            _services.AddScoped(typeof(IDermatologistWorkPlaceService), typeof(DermatologistWorkPlaceService));
            _services.AddScoped(typeof(IPharmacyPriceListService), typeof(PharmacyPriceListService));
            _services.AddScoped(typeof(IPharmacyStockService), typeof(PharmacyStockService));
            _services.AddScoped(typeof(IPharmacyReportsService), typeof(PharmacyReportsService));
            _services.AddScoped(typeof(IMedicalStaffService), typeof(MedicalStaffService));
            _services.AddScoped(typeof(IReportService), typeof(ReportService));
            _services.AddScoped(typeof(IPatientAllergyService), typeof(PatientAllergyService));
            _services.AddScoped(typeof(IMedicinePdfService), typeof(MedicinePdfService));
            _services.AddScoped(typeof(IGradeService), typeof(GradeService));
            _services.AddScoped(typeof(IMedicalStaffGradeService), typeof(MedicalStaffGradeService));
            _services.AddScoped(typeof(IERecipeService), typeof(ERecipeService));
            _services.AddScoped(typeof(IMedicineGradeService), typeof(MedicineGradeService));
            _services.AddScoped(typeof(IPromotionService), typeof(PromotionService));
            _services.AddScoped(typeof(IComplaintService<>), typeof(ComplaintService<>));
            _services.AddScoped(typeof(IComplaintAboutDermatologistService), typeof(ComplaintAboutDermatologistService));
            _services.AddScoped(typeof(IComplaintAboutPharmacistService), typeof(ComplaintAboutPharmacistService));
            _services.AddScoped(typeof(IComplaintAboutPharmacyService), typeof(ComplaintAboutPharmacyService));
            _services.AddScoped(typeof(IComplaintAnswerService), typeof(ComplaintAnswerService));
            _services.AddScoped(typeof(ILoyaltyPointsService), typeof(LoyaltyPointsService));
            _services.AddScoped(typeof(IPharmacyGradeService), typeof(PharmacyGradeService));
            _services.AddScoped(typeof(ILoyaltyProgramService), typeof(LoyaltyProgramService));
            _services.AddScoped(typeof(IAbsenceRequestService), typeof(AbsenceRequestService));
            _services.AddScoped(typeof(INotInStockService), typeof(NotInStockService));
            _services.AddScoped(typeof(IDiscountService), typeof(DiscountService));
        }
    }
}