using EmailService.Constracts;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Farmacio_Tests.UnitTests.ReservationServiceTests
{
    public class ReserveMedicinesTests
    {
        private readonly IReservationService _reservationService;
        private readonly Mock<IRepository<Reservation>> _reservationRepository;
        private readonly Mock<IPharmacyService> _pharmacyService;
        private readonly Mock<IPatientService> _patientService;
        private readonly Mock<IMedicineService> _medicineService;
        private readonly Mock<IDiscountService> _discountService;
        private readonly Mock<ITemplatesProvider> _templatesProvider;
        private readonly Mock<IEmailDispatcher> _emailDispatcher;

        public ReserveMedicinesTests()
        {
            _reservationRepository = new Mock<IRepository<Reservation>>();
            _pharmacyService = new Mock<IPharmacyService>();
            _patientService = new Mock<IPatientService>();
            _medicineService = new Mock<IMedicineService>();
            _discountService = new Mock<IDiscountService>();
            _templatesProvider = new Mock<ITemplatesProvider>();
            _emailDispatcher = new Mock<IEmailDispatcher>();

            _reservationService = new ReservationService(_pharmacyService.Object, _patientService.Object, _discountService.Object, _emailDispatcher.Object,
                _templatesProvider.Object, _medicineService.Object, _reservationRepository.Object);
        }

        [Fact]
        public void ReserveMedicines_ThrowsMissingEntityException_BecausePharmacyDoesntExist()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Throws<MissingEntityException>();

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                PharmacyId = new Guid("08d8f515-577c-4a9f-8d0c-b863603902e4")
            }, false);

            reserveMedicines.Should().Throw<MissingEntityException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowsMissingEntityException_BecausePatientDoesntExist()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => null);

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60")
            }, false);

            reserveMedicines.Should().Throw<MissingEntityException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowsBadLogicEXception_BecausePatientHasThreeOrMoreNegativePoints()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 3
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => true);

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60")
            }, false);

            reserveMedicines.Should().Throw<BadLogicException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowBadLogicException_BecauseLeftLessThan36HoursUntilPickupDeadline()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now
            }, false);

            reserveMedicines.Should().Throw<BadLogicException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowMissingEntityException_BecauseMedicineInReservationDoesntExist()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            _medicineService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Throws<MissingEntityException>();

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
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
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now.AddHours(37)
            }, false);

            reserveMedicines.Should().Throw<MissingEntityException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowBadLogicException_BecauseMedicineIsOnRecipeOnly()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            _medicineService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Medicine()
            {
                IsRecipeOnly = true
            });

            _pharmacyService.Setup(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid medicineId) => new MedicineInPharmacyDTO());

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-677c-4a9f-8d0c-b863603902e4"),
                    }
                },
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now.AddHours(37)
            }, false);


            reserveMedicines.Should().Throw<BadLogicException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ThrowMissingEntityException_BecausePharmacyDoesntHaveEnoughMedicineInStock()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            _medicineService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Medicine()
            {
                IsRecipeOnly = false
            });

            _pharmacyService.Setup(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid medicineId) => new MedicineInPharmacyDTO()
            {
                InStock = 2
            });

            Action reserveMedicines = () => _reservationService.CreateReservation(new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-677c-4a9f-8d0c-b863603902e4"),
                        Quantity = 3
                    }
                },
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now.AddHours(37)
            }, false);


            reserveMedicines.Should().Throw<MissingEntityException>();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveMedicines_ReturnNewReservation_IsRecipeOnlyFalse()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            _medicineService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Medicine()
            {
                IsRecipeOnly = false
            });

            _pharmacyService.Setup(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid medicineId) => new MedicineInPharmacyDTO()
            {
                InStock = 5
            });

            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-677c-4a9f-8d0c-b863603902e4"),
                        Quantity = 3
                    }
                },
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now.AddHours(37),
                Id = new Guid("08d8f518-58cc-41e9-810e-0a83d243cd60")
            };

            _reservationRepository.Setup(repository => repository.Create(reservation)).Returns((Reservation reservation) => reservation);


            var createdReservation = _reservationService.CreateReservation(reservation, false);

            createdReservation.Should().NotBeNull();
            createdReservation.Active.Should().BeTrue();
            
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Once);
        }


        [Fact]
        public void ReserveMedicines_ReturnNewReservation_IsRecipeOnlyTrue()
        {
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Patient
                {
                    NegativePoints = 0
                }
            });

            _patientService.Setup(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>())).Returns((Guid id) => false);

            _medicineService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Medicine()
            {
                IsRecipeOnly = false
            });

            _pharmacyService.Setup(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid medicineId) => new MedicineInPharmacyDTO()
            {
                InStock = 5
            });

            Reservation reservation = new Reservation
            {
                Medicines = new List<ReservedMedicine>()
                {
                    new ReservedMedicine
                    {
                        MedicineId = new Guid("08d8f514-677c-4a9f-8d0c-b863603902e4"),
                        Quantity = 3
                    }
                },
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PickupDeadline = DateTime.Now.AddHours(37),
                Id = new Guid("08d8f518-58cc-41e9-810e-0a83d243cd60")
            };

            _reservationRepository.Setup(repository => repository.Create(reservation)).Returns((Reservation reservation) => reservation);


            var createdReservation = _reservationService.CreateReservation(reservation, true);

            createdReservation.Should().NotBeNull();
            createdReservation.Active.Should().BeTrue();

            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.HasExceededLimitOfNegativePoints(It.IsAny<Guid>()), Times.Once);
            _medicineService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ReadMedicine(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.ChangeStockFor(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _reservationRepository.Verify(repository => repository.Create(It.IsAny<Reservation>()), Times.Once);
        }
    }
}
