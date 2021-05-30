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
using System.Collections.Generic;
using Xunit;

namespace Farmacio_Tests.UnitTests.AppointmentServiceTests
{
    public class CreatePharmacistAppointmentTests
    {
        private readonly IAppointmentService _appointmentService;
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly Mock<IPharmacyService> _pharmacyService;
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<IPatientService> _patientService;
        private readonly Mock<IDiscountService> _discountService;
        private readonly Mock<ITemplatesProvider> _templatesProvider;
        private readonly Mock<IEmailDispatcher> _emailDispatcher;
        private readonly Mock<IAbsenceRequestService> _absenceRequestService;

        public CreatePharmacistAppointmentTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _pharmacyService = new Mock<IPharmacyService>();
            _accountService = new Mock<IAccountService>();
            _patientService = new Mock<IPatientService>();
            _discountService = new Mock<IDiscountService>();
            _templatesProvider = new Mock<ITemplatesProvider>();
            _emailDispatcher = new Mock<IEmailDispatcher>();
            _absenceRequestService = new Mock<IAbsenceRequestService>();
            _absenceRequestService
                .Setup(service => service.IsMedicalStaffAbsent(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(false);
            _appointmentService = new AppointmentService(_appointmentRepository.Object, _pharmacyService.Object, _accountService.Object, null,
                _patientService.Object, _discountService.Object, _emailDispatcher.Object, _templatesProvider.Object, null, null, _absenceRequestService.Object);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecauseDateTimePast()
        {
            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                DateTime = DateTime.Now.AddHours(-1),
            });

            createAppointment.Should().Throw<BadLogicException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsMissingEntityException_BecausePatientNotFound()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Pharmacist() });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => null);

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                DateTime = DateTime.Now.AddDays(3),
            });

            createAppointment.Should().Throw<MissingEntityException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Never);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsMissingEntityException_BecausePharmacyDoesntExist()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Pharmacist() });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Throws<MissingEntityException>();

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e5"),
                DateTime = DateTime.Today.AddDays(3),
            });

            createAppointment.Should().Throw<MissingEntityException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecausePharmacistDoesntWorkInThatPharmacy()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e5"),
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Now.AddDays(3),
            });

            createAppointment.Should().Throw<BadLogicException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfWorkTime()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(15),   // 15:00
                Duration = 10
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfPharmacistsAnotherAppointment()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _appointmentRepository.Setup(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>())).Returns(() => new List<Appointment>
            {
                new Appointment
                {
                    MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                    DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                    Duration = 10
                }
            });

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsInvalidAppointmentDateTimeException_BecauseOfPatientsAnotherAppointment()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _appointmentRepository.Setup(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>())).Returns(() =>
                new List<Appointment>());

            _appointmentRepository.Setup(repository => repository.ReadForPatient(It.IsAny<Guid>())).Returns(() =>
                new List<Appointment>
                {
                    new Appointment
                    {
                        PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                        DateTime = DateTime.Today.AddDays(2).AddHours(10), // 10:00
                        Duration = 10
                    }
                });

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
            });

            createAppointment.Should().Throw<InvalidAppointmentDateTimeException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Exactly(1));
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsMissingEntityException_BecausePriceListNotFound()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _appointmentRepository.Setup(repository => repository.Read()).Returns(() => new List<Appointment>());
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _pharmacyService.Setup(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>())).Throws<MissingEntityException>();

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
                Price = null
            });

            createAppointment.Should().Throw<MissingEntityException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Once);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Exactly(1));
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [Fact]
        public void CreatePharmacistAppointment_ThrowsBadLogicException_BecauseInvalidPrice()
        {
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _appointmentRepository.Setup(repository => repository.Read()).Returns(() => new List<Appointment>());
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _discountService.Setup(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid patientId) => 3);

            Action createAppointment = () => _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
                Price = 10000000
            });

            createAppointment.Should().Throw<BadLogicException>();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Exactly(1));
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [Fact]
        public void CreatePharmacistAppointment_ReturnsNewAppointment()
        {
            _appointmentRepository.Setup(repository => repository.OpenTransaction()).Returns(new DummyTransaction());
            _accountService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account
            {
                User = new Pharmacist
                {
                    PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                    WorkTime = new WorkTime
                    {
                        From = new DateTime(2020, 1, 1, 8, 0, 0),   // 8:00
                        To = new DateTime(2020, 1, 1, 13, 0, 0),    // 13:00
                    }
                }
            });
            _patientService.Setup(service => service.ReadByUserId(It.IsAny<Guid>())).Returns((Guid id) => new Account { User = new Patient() });
            _appointmentRepository.Setup(repository => repository.Read()).Returns(() => new List<Appointment>());
            _pharmacyService.Setup(service => service.TryToRead(It.IsAny<Guid>())).Returns((Guid id) => new Pharmacy());
            _discountService.Setup(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Guid pharmacyId, Guid patientId) => 3);
            _appointmentRepository.Setup(repository => repository.Create(It.IsAny<Appointment>())).Returns((Appointment appointment) => appointment);

            var createdAppointment = _appointmentService.CreatePharmacistAppointment(new CreateAppointmentDTO
            {
                MedicalStaffId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd61"),
                PatientId = new Guid("08d8f513-58cc-41e9-810e-0a83d243cd60"),
                PharmacyId = new Guid("08d8f514-577c-4a9f-8d0c-b863603902e4"),
                DateTime = DateTime.Today.AddDays(2).AddHours(10),   // 10:00
                Duration = 20,
                Price = 300
            });

            createdAppointment.Should().NotBeNull();
            createdAppointment.Active.Should().BeTrue();

            _accountService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.TryToRead(It.IsAny<Guid>()), Times.Once);
            _pharmacyService.Verify(service => service.GetPriceOfPharmacistConsultation(It.IsAny<Guid>()), Times.Never);
            _discountService.Verify(service => service.ReadDiscountFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _patientService.Verify(service => service.ReadByUserId(It.IsAny<Guid>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.Create(It.IsAny<Appointment>()), Times.Once);
            _appointmentRepository.Verify(repository => repository.ReadForPatient(It.IsAny<Guid>()), Times.Exactly(1));
            _appointmentRepository.Verify(repository => repository.ReadForMedicalStaff(It.IsAny<Guid>()), Times.Exactly(1));
        }
    }
}