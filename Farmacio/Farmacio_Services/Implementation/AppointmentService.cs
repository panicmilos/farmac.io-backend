using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class AppointmentService : CrudService<Appointment>, IAppointmentService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IAccountService _accountService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IPatientService _patientService;
        private readonly IDiscountService _discountService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IReportService _reportService;
        private readonly IERecipeService _eRecipeService;
        private readonly IPromotionService _promotionService;

        public AppointmentService(IRepository<Appointment> repository
            , IPharmacyService pharmacyService, IAccountService accountService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , IPatientService patientService
            , IDiscountService discountService
            , IEmailDispatcher emailDispatcher
            , ITemplatesProvider templateProvider
            , IReportService reportService
            , IERecipeService eRecipeService
            , IPromotionService promotionService) : base(repository)

        {
            _pharmacyService = pharmacyService;
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _patientService = patientService;
            _discountService = discountService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templateProvider;
            _reportService = reportService;
            _eRecipeService = eRecipeService;
            _promotionService = promotionService;
        }

        public IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId)
        {
            return Read().ToList().Where(a => a.MedicalStaff.Id == medicalStaffId).ToList();
        }

        public IEnumerable<Appointment> ReadForMedicalStaffInPharmacy(Guid medicalStaffId, Guid pharmacyId)
        {
            return ReadForMedicalStaff(medicalStaffId).Intersect(ReadForPharmacy(pharmacyId));
        }

        public IEnumerable<Appointment> ReadForPharmacy(Guid pharmacyId)
        {
            return Read().Where(appointment => appointment.PharmacyId == pharmacyId).ToList();
        }

        public IEnumerable<Appointment> ReadForDermatologistsInPharmacy(Guid pharmacyId)
        {
            return ReadForPharmacy(pharmacyId)
                .Where(appointment =>
                    _accountService.ReadByUserId(appointment.MedicalStaffId)?.Role == Role.Dermatologist);
        }

        public Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointmentDTO)
        {
            if (appointmentDTO.DateTime < DateTime.Now)
                throw new BadLogicException("The given date and time are in the past.");

            var pharmacy = _pharmacyService.TryToRead(appointmentDTO.PharmacyId);

            var workPlace = _dermatologistWorkPlaceService
                .GetWorkPlaceInPharmacyFor(appointmentDTO.MedicalStaffId, pharmacy.Id);
            if (workPlace == null)
                throw new MissingEntityException("Dermatologist work place for the given pharmacy id not found.");

            ValidateAppointmentDateTime(appointmentDTO, workPlace.WorkTime, "The given date-time and duration do not overlap with dermatologist's work time.",
                "Dermatologist already has an appointment defined on the given date-time.");

            bool withPatient = appointmentDTO.PatientId != null;

            if (withPatient)
                ValidateTimeForPatient(appointmentDTO.PatientId.Value, appointmentDTO.DateTime, appointmentDTO.Duration);

            var originalPrice = appointmentDTO.Price ?? _pharmacyService.GetPriceOfDermatologistExamination(pharmacy.Id);
            var price = appointmentDTO.PatientId != null ? DiscountUtils.ApplyDiscount(originalPrice, _discountService
                .ReadDiscountFor(appointmentDTO.PharmacyId, appointmentDTO.PatientId.Value)) : originalPrice;
            if (price <= 0 || price > 999999)
                throw new BadLogicException("Price must be a valid number between 0 and 999999.");

            var newAppointment = Create(new Appointment
            {
                PharmacyId = appointmentDTO.PharmacyId,
                MedicalStaffId = appointmentDTO.MedicalStaffId,
                DateTime = appointmentDTO.DateTime,
                Duration = appointmentDTO.Duration,
                Price = price,
                OriginalPrice = originalPrice,
                PatientId = appointmentDTO.PatientId,
                IsReserved = withPatient
            });

            if (withPatient)
            {
                var patient = _patientService.ReadByUserId(appointmentDTO.PatientId.Value);

                var email = _templatesProvider.FromTemplate<Email>("Appointment", new { Name = patient.User.FirstName, Date = newAppointment.DateTime.ToString("dd-MM-yyyy HH:mm") });
                _emailDispatcher.Dispatch(email);
            }
            return newAppointment;
        }

        private void ValidateAppointmentDateTime(CreateAppointmentDTO appointment, WorkTime workTime, String medicalStaffDontWorkMMessage, String medicalStaffIsBusyMessage)
        {
            var from = appointment.DateTime;
            var to = @from.AddMinutes(appointment.Duration);
            if (!TimeIntervalUtils.TimeIntervalTimesOverlap(@from, to, workTime.From, workTime.To))
                throw new InvalidAppointmentDateTimeException(
                    medicalStaffDontWorkMMessage);

            var overlap = ReadForMedicalStaff(appointment.MedicalStaffId).FirstOrDefault(a =>
            {
                var existingFrom = a.DateTime;
                var existingTo = a.DateTime.AddMinutes(a.Duration);

                var datesAreEqual = existingFrom.Date == @from.Date;
                return datesAreEqual &&
                       TimeIntervalUtils.TimeIntervalTimesOverlap(@from, to, existingFrom, existingTo);
            });
            if (overlap != null)
                throw new InvalidAppointmentDateTimeException(
                    medicalStaffIsBusyMessage);
        }

        public Appointment MakeAppointmentWithDermatologist(MakeAppointmentWithDermatologistDTO appointmentRequest)
        {
            var appointmentWithDermatologist = base.Read(appointmentRequest.AppointmentId);
            if (appointmentWithDermatologist == null)
                throw new MissingEntityException("The given appointment does not exist in the system.");

            if (_patientService.Read().Where(account => account.UserId == appointmentRequest.PatientId) == null)
                throw new MissingEntityException("The given patient does not exist in the system.");

            if (appointmentWithDermatologist.IsReserved)
                throw new BadLogicException("The given appointment is already reserved.");

            if (_patientService.HasExceededLimitOfNegativePoints(appointmentRequest.PatientId))
                throw new BadLogicException("The given patient has 3 or more negative points.");

            if (appointmentWithDermatologist.DateTime < DateTime.Now)
                throw new BadLogicException("The given appointment is in the past.");

            ValidateTimeForPatient(appointmentRequest.PatientId, appointmentWithDermatologist.DateTime, appointmentWithDermatologist.Duration);

            appointmentWithDermatologist.IsReserved = true;
            appointmentWithDermatologist.PatientId = appointmentRequest.PatientId;
            appointmentWithDermatologist.Price = DiscountUtils.ApplyDiscount(appointmentWithDermatologist.OriginalPrice,
                _discountService.ReadDiscountFor(appointmentWithDermatologist.PharmacyId,
                    appointmentWithDermatologist.PatientId.Value));

            var email = _templatesProvider.FromTemplate<Email>("Appointment", new { Name = appointmentWithDermatologist.Patient.FirstName, Date = appointmentWithDermatologist.DateTime.ToString("dd-MM-yyyy HH:mm") });
            _emailDispatcher.Dispatch(email);
            return base.Update(appointmentWithDermatologist);
        }

        public IEnumerable<Appointment> SortAppointments(IEnumerable<Appointment> appointments, string criteria, bool isAsc)
        {
            var sortingCriteria = new Dictionary<string, Func<Appointment, object>>()
            {
                { "grade", a => a.MedicalStaff.AverageGrade },
                { "price", a => a.Price },
                { "duration", a => a.Duration },
                { "date", a => a.DateTime}
            };

            if (sortingCriteria.TryGetValue(criteria ?? "", out var sortingCriterion))
                appointments = isAsc ? appointments.OrderBy(sortingCriterion) : appointments.OrderByDescending(sortingCriterion);

            return appointments;
        }

        public IEnumerable<Appointment> ReadForPatients(Guid patientId)
        {
            if (_patientService.Read().Where(account => account.UserId == patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }
            return Read().ToList().Where(appointment =>
                                appointment.PatientId == patientId && appointment.IsReserved && appointment.DateTime > DateTime.Now && appointment.MedicalStaff is Dermatologist);
        }

        public Appointment CancelAppointmentWithDermatologist(Guid appointmentId)
        {
            var appointment = base.TryToRead(appointmentId);
            if (!appointment.IsReserved)
                throw new BadLogicException("Given appointment is not reserved.");
            if (DateTime.Now.AddHours(24) > appointment.DateTime)
                throw new BadLogicException("It is not possible to cancel an appointment if there are less than 24 hours left before the start.");
            if (appointment.DateTime < DateTime.Now)
                throw new BadLogicException("It is not possible to cancel an appointment which date and time in the past.");
            appointment.IsReserved = false;
            appointment.PatientId = null;
            appointment.Patient = null;
            appointment.Price = appointment.OriginalPrice;

            base.Update(appointment);
            return appointment;
        }

        public IEnumerable<Appointment> ReadPatientsHistoryOfVisitsToDermatologist(Guid patientId)
        {
            if (_patientService.Read().Where(account => account.UserId == patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }
            return Read().ToList().Where(appointment =>
                appointment.PatientId == patientId && appointment.IsReserved && appointment.DateTime < DateTime.Now && appointment.MedicalStaff is Dermatologist);
        }

        public Report CreateReport(CreateReportDTO reportDTO)
        {
            var appointment = base.TryToRead(reportDTO.AppointmentId);
            if (!appointment.IsReserved)
                throw new BadLogicException("Given appointment is not reserved.");
            if (appointment.ReportId != null)
                throw new BadLogicException("Given appointment is already reported.");
            var report = new Report
            {
                Notes = reportDTO.Notes,
                TherapyDurationInDays = reportDTO.TherapyDurationInDays,
            };
            if (reportDTO.PrescribedMedicines.Count == 0)
            {
                report = _reportService.Create(report);
                appointment.ReportId = report.Id;
                base.Update(appointment);
                return report;
            }
            ERecipe recipe = new ERecipe
            {
                IssuingDate = DateTime.Now,
                PatientId = appointment.PatientId.Value,
                Medicines = new List<ERecipeMedicine>()
            };
            foreach (var prescribedMedicine in reportDTO.PrescribedMedicines)
            {
                var medicineInPharmacy = _pharmacyService.ReadMedicine(appointment.PharmacyId, prescribedMedicine.MedicineId);
                if (medicineInPharmacy.InStock < prescribedMedicine.Quantity)
                    throw new MissingEntityException($"Pharmacy does not have enough {medicineInPharmacy.Name}.");
                recipe.Medicines.Add(new ERecipeMedicine
                {
                    MedicineId = prescribedMedicine.MedicineId,
                    Quantity = prescribedMedicine.Quantity
                });
            }
            foreach (var prescribed in reportDTO.PrescribedMedicines)
                _pharmacyService.ChangeStockFor(appointment.PharmacyId, prescribed.MedicineId, prescribed.Quantity * -1);
            recipe = _eRecipeService.Create(recipe);
            report.ERecipeId = recipe.Id;
            report = _reportService.Create(report);
            appointment.ReportId = report.Id;
            base.Update(appointment);
            return report;
        }

        public IEnumerable<Appointment> ReadReservedButUnreportedForMedicalStaff(Guid medicalStaffId)
        {
            return ReadForMedicalStaff(medicalStaffId).Where(appointment => appointment.IsReserved
                                                                && appointment.ReportId == null
                                                                && appointment.PatientId != null).ToList();
        }

        public Report NotePatientDidNotShowUp(CreateReportDTO reportDTO)
        {
            var report = CreateReport(reportDTO);
            var appointment = base.TryToRead(reportDTO.AppointmentId);
            var patient = appointment.Patient;
            patient.NegativePoints += 1;
            var patientAccount = _accountService.ReadByUserId(patient.Id);
            if (patientAccount == null)
                throw new MissingEntityException("Patient not found.");
            _patientService.Update(patientAccount);
            return report;
        }

        public Appointment CreatePharmacistAppointment(CreateAppointmentDTO appointmentDTO)
        {
            if (appointmentDTO.DateTime < DateTime.Now)
                throw new BadLogicException("The given date and time are in the past.");

            var medicalAccount = _accountService.ReadByUserId(appointmentDTO.MedicalStaffId);

            var pharmacist = (Pharmacist)medicalAccount.User;

            if (pharmacist.PharmacyId != appointmentDTO.PharmacyId)
                throw new BadLogicException("Pharmacist must work in that pharmacy.");

            ValidateAppointmentDateTime(appointmentDTO, pharmacist.WorkTime, "The given date-time and duration do not overlap with pharmacist's work time.",
                "Pharmacist already has an appointment defined on the given date-time.");

            ValidateTimeForPatient(appointmentDTO.PatientId.Value, appointmentDTO.DateTime, appointmentDTO.Duration);

            var pharmacy = _pharmacyService.TryToRead(appointmentDTO.PharmacyId);

            var originalPrice = appointmentDTO.Price ?? _pharmacyService.GetPriceOfPharmacistConsultation(pharmacy.Id);
            var price = appointmentDTO.PatientId != null
                ? DiscountUtils.ApplyDiscount(originalPrice,
                    _discountService.ReadDiscountFor(appointmentDTO.PharmacyId, appointmentDTO.PatientId.Value))
                : originalPrice;
            if (price <= 0 || price > 999999)
                throw new BadLogicException("Price must be a valid number between 0 and 999999.");
            
            var appointmentWithPharmacist = Create(new Appointment
            {
                PharmacyId = appointmentDTO.PharmacyId,
                MedicalStaffId = appointmentDTO.MedicalStaffId,
                DateTime = appointmentDTO.DateTime,
                Duration = appointmentDTO.Duration,
                Price = price,
                OriginalPrice = originalPrice,
                PatientId = appointmentDTO.PatientId,
                IsReserved = true
            });

            var patient = _patientService.ReadByUserId(appointmentDTO.PatientId.Value);

            var email = _templatesProvider.FromTemplate<Email>("Consultation", new { Name = patient.User.FirstName, Date = appointmentWithPharmacist.DateTime.ToString("dd-MM-yyyy HH:mm") });
            _emailDispatcher.Dispatch(email);

            return appointmentWithPharmacist;
        }

        public IEnumerable<Account> ReadPharmacistsForAppointment(IEnumerable<Account> pharmacists, SearhSortParamsForAppointments searchParams)
        {
            return pharmacists
            .Where(pharmacistAccount =>
            {
                var pharmacist = pharmacistAccount.User as Pharmacist;
                return pharmacist.WorkTime.From.TimeOfDay <= searchParams.ConsultationDateTime.TimeOfDay &&
                searchParams.ConsultationDateTime.AddMinutes(searchParams.Duration).TimeOfDay <= pharmacist.WorkTime.To.TimeOfDay;
            })
            .Where(pharmacistAccount =>
            {
                var pharmacist = pharmacistAccount.User as Pharmacist;
                var overlapingAppointments = ReadForMedicalStaff(pharmacist.Id)
                .Where(appointment =>
                    appointment.MedicalStaffId == pharmacist.Id &&
                    appointment.DateTime.Date == searchParams.ConsultationDateTime.Date &&
                    TimeIntervalUtils.TimeIntervalTimesOverlap(searchParams.ConsultationDateTime, searchParams.ConsultationDateTime.AddMinutes(searchParams.Duration),
                    appointment.DateTime, appointment.DateTime.AddMinutes(appointment.Duration)));
                return overlapingAppointments.Count() == 0;
            });
        }

        public IEnumerable<Appointment> ReadFuturePharmacistsAppointmentsFor(Guid patientId)
        {
            if (_patientService.ReadByUserId(patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }
            return Read().ToList().Where(appointment =>
                                appointment.PatientId == patientId && appointment.IsReserved && appointment.DateTime > DateTime.Now && appointment.MedicalStaff is Pharmacist);
        }

        public Appointment CancelAppointmentWithPharmacist(Guid appointmentId)
        {
            var appointment = base.TryToRead(appointmentId);

            if (DateTime.Now.AddHours(24) > appointment.DateTime)
                throw new BadLogicException("It is not possible to cancel an appointment if there are less than 24 hours left before the start.");

            if (appointment.DateTime < DateTime.Now)
                throw new BadLogicException("It is not possible to cancel an appointment which date and time are in the past.");

            base.Delete(appointment.Id);

            return appointment;
        }

        public bool DidPatientHaveAppointmentWithMedicalStaff(Guid patientId, Guid medicalStaffUserId)
        {
            var patient = _patientService.TryToRead(patientId);
            return ReadForMedicalStaff(medicalStaffUserId).Where(appointment => appointment.IsReserved
                             && appointment.PatientId == patient.UserId && appointment.DateTime < DateTime.Now).Count() != 0;
        }

        public Appointment CreateAnotherAppointmentByMedicalStaff(CreateAppointmentDTO appointment)
        {
            var medicalAccount = _accountService.ReadByUserId(appointment.MedicalStaffId);
            if (medicalAccount.Role == Role.Dermatologist)
                return CreateDermatologistAppointment(appointment);
            else if (medicalAccount.Role == Role.Pharmacist)
                return CreatePharmacistAppointment(appointment);
            return new Appointment();
        }

        private void ValidateTimeForPatient(Guid patientId, DateTime dateTime, int duration)
        {
            var patientsAppointments = base.Read().Where(appointment => appointment.PatientId == patientId);
            foreach (var pa in patientsAppointments)
            {
                if (pa.DateTime.Date == dateTime.Date && TimeIntervalUtils.TimeIntervalTimesOverlap(
                    pa.DateTime, pa.DateTime.AddMinutes(pa.Duration), dateTime,
                    dateTime.AddMinutes(duration)))
                    throw new BadLogicException("The given appointment overlaps with the already reserved appointment of the patient.");
            }
        }

        public IEnumerable<AppointmentAsEvent> ReadAppointmentsForCalendar(Guid medicalStaffId)
        {
            return ReadForMedicalStaff(medicalStaffId).Select(appointment => new AppointmentAsEvent
            {
                Id = appointment.Id,
                Start = appointment.DateTime,
                End = appointment.DateTime.AddMinutes(appointment.Duration),
                Title = $"{appointment.Patient?.FirstName} {appointment.Patient?.LastName}",
                PharmacyName = appointment.Pharmacy.Name,
                IsReported = appointment.ReportId != null
            });
        }

        public IEnumerable<Appointment> ReadFor(Guid patientId)
        {
            if (_patientService.ReadByUserId(patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            return Read().Where(appointment => appointment.PatientId == patientId).ToList();
        }

        public IEnumerable<Appointment> ReadPatientsHistoryOfVisitingPharmacists(Guid patientId)
        {
            if (_patientService.ReadByUserId(patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }
            return ReadFor(patientId).Where(appointment =>
                appointment.IsReserved && appointment.DateTime < DateTime.Now && appointment.MedicalStaff is Pharmacist);
        }
    }
}