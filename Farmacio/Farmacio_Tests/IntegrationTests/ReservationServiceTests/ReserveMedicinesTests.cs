using EmailService.Constracts;
using EmailService.Implementation;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Farmacio_Tests.IntegrationTests.ReservationServiceTests
{
    public class ReserveMedicinesTests : FarmacioTestBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IPharmacyService _pharmacyService;
        private readonly IPatientService _patientService;
        private readonly IMedicineService _medicineService;
        private readonly IDiscountService _discountService;
        private readonly Mock<ITemplatesProvider> _templatesProvider;
        private readonly Mock<IEmailDispatcher> _emailDispatcher;

        public ReserveMedicinesTests()
        {
            var pharmacyOrderService = new CrudService<PharmacyOrder>(new Repository<PharmacyOrder>(context));
            var pharmacyPriceListService = new PharmacyPriceListService(new Repository<PharmacyPriceList>(context));
            var pharmacyStockService = new PharmacyStockService(pharmacyOrderService, new Repository<PharmacyMedicine>(context));

            _templatesProvider = new Mock<ITemplatesProvider>();
            _emailDispatcher = new Mock<IEmailDispatcher>();
            _reservationRepository = new Repository<Reservation>(context);
            _pharmacyService = new PharmacyService(pharmacyPriceListService, pharmacyStockService, new Repository<Pharmacy>(context));
            _patientService = new PatientService(new EmailVerificationService(null, _emailDispatcher.Object, _templatesProvider.Object), new AccountRepository(context));
            _medicineService = new MedicineService(null, null, _pharmacyService, pharmacyStockService, null, null, new Repository<Medicine>(context));
            _discountService = new DiscountService(new LoyaltyProgramService(_patientService, new Repository<LoyaltyProgram>(context)),
                new PromotionService(_pharmacyService, _emailDispatcher.Object, _templatesProvider.Object, new Repository<Promotion>(context), null));

            _reservationService = new ReservationService(_pharmacyService, _patientService, _discountService, _emailDispatcher.Object,
                _templatesProvider.Object, _medicineService, _reservationRepository);
        }

        [Fact]
        public void ReserveMedicines_ThrowsMissingEntityException_BecausePharmacyDoesntExist()
        {
            Reservation reservation = new Reservation
            {
                PharmacyId = new Guid("09d8f515-577c-4a9f-8d0c-b863603902e4")
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowsMissingEntityException_BecausePatientDoesntExist()
        {
            Reservation reservation = new Reservation
            {
                PharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3"),
                PatientId = new Guid("08d8f513-58cc-42e9-810e-0a83d243cd60")
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowsBadLogicEXception_BecausePatientHasThreeOrMoreNegativePoints()
        {
            Reservation reservation = new Reservation
            {
                PharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3"),
                PatientId = new Guid("08d91521-5dc6-45c3-81ec-26b64d85b5ea")
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowBadLogicException_BecauseLeftLessThan36HoursUntilPickupDeadline()
        {
            Reservation reservation = new Reservation
            {
                PharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowMissingEntityException_BecauseMedicineInReservationDoesntExist()
        {
            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-677c-4a9f-8d0c-b863603902e4")
                    },
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-777c-4a9f-8d0c-b863603902e4")
                    }
                },
                PharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now.AddHours(37)
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowBadLogicException_BecauseMedicineIsOnRecipeOnly()
        {
            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d91521-5c05-422b-8f14-df14e1ee1016"),
                        Quantity = 1
                    }
                },
                PharmacyId = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now.AddHours(37)
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void ReserveMedicines_ThrowMissingEntityException_BecausePharmacyDoesntHaveEnoughMedicineInStock()
        {
            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d91521-5bf4-4a5e-8740-d68fcde43c58"),
                        Quantity = 7
                    }
                },
                PharmacyId = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now.AddHours(37)
            };
            bool checkIsERecipe = false;

            Action reserveMedicines = () => _reservationService.CreateReservation(reservation, checkIsERecipe);

            reserveMedicines.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void ReserveMedicines_ReturnNewReservation_IsRecipeOnlyFalse()
        {
            using var transaction = _reservationRepository.OpenTransaction();

            bool checkIsERecipe = false;
            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d91521-5bf4-4a5e-8740-d68fcde43c58"),
                        Quantity = 3
                    }
                },
                PharmacyId = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now.AddHours(37)
            };

            var createdReservation = _reservationService.CreateReservation(reservation, checkIsERecipe);

            createdReservation.Should().NotBeNull();
            createdReservation.Active.Should().BeTrue();

            transaction.Rollback();
        }

        [Fact]
        public void ReserveMedicines_ReturnNewReservation_IsRecipeOnlyTrue()
        {
            using var transaction = _reservationRepository.OpenTransaction();

            bool checkIsERecipe = true;
            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d91521-5c05-422b-8f14-df14e1ee1016"),
                        Quantity = 2
                    }
                },
                PharmacyId = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
                PatientId = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
                PickupDeadline = DateTime.Now.AddHours(37)
            };

            var createdReservation = _reservationService.CreateReservation(reservation, checkIsERecipe);

            createdReservation.Should().NotBeNull();
            createdReservation.Active.Should().BeTrue();

            transaction.Rollback();
        }
    }
}