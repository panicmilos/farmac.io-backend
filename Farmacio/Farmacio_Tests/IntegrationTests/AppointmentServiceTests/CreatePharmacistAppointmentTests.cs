using EmailService.Constracts;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using Moq;
using System;
using Xunit;

namespace Farmacio_Tests.IntegrationTests.AppointmentServiceTests
{
    public class CreatePharmacistAppointmentTests : FarmacioTestBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPharmacyService _pharmacyService;
        private readonly IAccountService _accountService;
        private readonly IPatientService _patientService;
        private readonly IDiscountService _discountService;
        private readonly Mock<ITemplatesProvider> _templatesProvider;
        private readonly Mock<IEmailDispatcher> _emailDispatcher;

        public CreatePharmacistAppointmentTests()
        {
            var pharmacyPriceListService = new PharmacyPriceListService(new Repository<PharmacyPriceList>(context));
            var accountRepository = new AccountRepository(context);
            _templatesProvider = new Mock<ITemplatesProvider>();
            _emailDispatcher = new Mock<IEmailDispatcher>();
            var emailVerificationService = new EmailVerificationService(null, _emailDispatcher.Object, _templatesProvider.Object);
            _appointmentRepository = new AppointmentRepository(context);
            _pharmacyService = new PharmacyService(pharmacyPriceListService, null, new Repository<Pharmacy>(context));
            var promotionService = new PromotionService(_pharmacyService, _emailDispatcher.Object, _templatesProvider.Object, new Repository<Promotion>(context), null);
            _accountService = new AccountService(emailVerificationService, accountRepository);
            _patientService = new PatientService(emailVerificationService, accountRepository);
            var loyaltyProgramService = new LoyaltyProgramService(_patientService, new Repository<LoyaltyProgram>(context));
            _discountService = new DiscountService(loyaltyProgramService, promotionService);
            _appointmentService = new AppointmentService(_appointmentRepository, _pharmacyService, _accountService, null,
                _patientService, _discountService, _emailDispatcher.Object, _templatesProvider.Object, null, null);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecauseDateTimePast()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                DateTime = DateTime.Now.AddHours(-1),
            });

            createAppointment.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsMissingEntityException_BecausePatientNotFound()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                DateTime = DateTime.Now.AddDays(3),
            });

            createAppointment.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsMissingEntityException_BecausePharmacyDoesntExist()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e5"),
                DateTime = DateTime.Today.AddDays(3),
            });

            createAppointment.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecausePharmacistDoesntWorkInThatPharmacy()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
                DateTime = DateTime.Now.AddDays(3),
            });

            createAppointment.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfWorkTime()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(15),   // 15:00
                Duration = 10
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfPharmacistsAnotherAppointment()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = new DateTime(2022, 7, 1, 9, 0, 0),   // 9:00
                Duration = 20
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfPatientsAnotherAppointment()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = new DateTime(2022, 6, 30, 9, 0, 0),   // 9:00
                Duration = 20,
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecauseInvalidPrice()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
                Price = 10000000
            });

            createAppointment.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void CreatePharmacistAppointment_ReturnsNewAppointment()
        {
            using var transaction = _appointmentRepository.OpenTransaction();

            var createdAppointment = _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
                Price = 300
            });

            createdAppointment.Should().NotBeNull();
            createdAppointment.Active.Should().BeTrue();

            transaction.Rollback();
        }
    }
}