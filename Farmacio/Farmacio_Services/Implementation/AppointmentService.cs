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
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        private readonly IPatientService _patientService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IReportService _reportService;
        private readonly IERecipeService _eRecipeService;


        public AppointmentService(IRepository<Appointment> repository
            , IPharmacyService pharmacyService, IAccountService accountService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , IPharmacyPriceListService pharmacyPriceListService
            , IPatientService patientService
            , IEmailDispatcher emailDispatcher
            , ITemplatesProvider templateProvider
            , IReportService reportService
            , IERecipeService eRecipeService) : base(repository)

        {
            _pharmacyService = pharmacyService;
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _pharmacyPriceListService = pharmacyPriceListService;
            _patientService = patientService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templateProvider;
            _reportService = reportService;
            _eRecipeService = eRecipeService;
        }

        public IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId)
        {
            return Read().ToList().Where(a => a.MedicalStaff.Id == medicalStaffId).ToList();
        }

        public IEnumerable<Appointment> ReadForDermatologistsInPharmacy(Guid pharmacyId)
        {
            return Read()
                .ToList()
                .Where(a => a.PharmacyId == pharmacyId &&
                            _accountService.Read().ToList()
                                .FirstOrDefault(acc => acc.UserId == a.MedicalStaffId)?.Role == Role.Dermatologist)
                .ToList();
        }

        public Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointment)
        {
            var pharmacy = _pharmacyService.TryToRead(appointment.PharmacyId);

            var workPlace = _dermatologistWorkPlaceService
                .GetWorkPlaceInPharmacyFor(appointment.MedicalStaffId, pharmacy.Id);
            if (workPlace == null)
                throw new MissingEntityException("Dermatologist work place for the given pharmacy id not found.");
            
            ValidateAppointmentDateTime(appointment, workPlace.WorkTime, "The given date-time and duration do not overlap with dermatologist's work time.",
                "Dermatologist already has an appointment defined on the given date-time.");

            var priceList = _pharmacyPriceListService.ReadForPharmacy(pharmacy.Id);
            if(priceList == null)
                throw new MissingEntityException("Price list not found for the given pharmacy.");
            
            var price = appointment.Price ?? priceList.ExaminationPrice;
            if(price <= 0 || price > 999999)
                throw new BadLogicException("Price must be a valid number between 0 and 999999.");

            return Create(new Appointment
            {
                PharmacyId = appointment.PharmacyId,
                MedicalStaffId = appointment.MedicalStaffId,
                DateTime = appointment.DateTime,
                Duration = appointment.Duration,
                Price = price
            });
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
            if(appointmentWithDermatologist == null)
                throw new MissingEntityException("The given appointment does not exist in the system.");

            if(_patientService.Read().Where(account => account.UserId == appointmentRequest.PatientId) == null)
                throw new MissingEntityException("The given patient does not exixst in the system.");

            if (appointmentWithDermatologist.IsReserved)
                throw new BadLogicException("The given appointment is already reserved.");

            if (_patientService.HasExceededLimitOfNegativePoints(appointmentRequest.PatientId))
                throw new BadLogicException("The given patient has 3 or more negative points.");

            if(appointmentWithDermatologist.DateTime < DateTime.Now)
                throw new BadLogicException("The given appointment is in the past.");

            var patientsAppointments = base.Read().Where(appointment => appointment.PatientId == appointmentRequest.PatientId);
            foreach(var appointment in patientsAppointments)
            {
                if(appointment.DateTime.Date == appointmentWithDermatologist.DateTime.Date && TimeIntervalUtils.TimeIntervalTimesOverlap(appointment.DateTime, 
                    appointment.DateTime.AddMinutes(appointment.Duration), appointmentWithDermatologist.DateTime,
                    appointmentWithDermatologist.DateTime.AddMinutes(appointmentWithDermatologist.Duration)))
                    throw new BadLogicException("The given appointment overlaps with the already reserved appointment of the patient.");
            }

            appointmentWithDermatologist.IsReserved = true;
            appointmentWithDermatologist.PatientId = appointmentRequest.PatientId;
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
                { "duration", a => a.Duration }
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
            if(!appointment.IsReserved)
                throw new BadLogicException("Given appointment is not reserved.");
            if(DateTime.Now.AddHours(24) > appointment.DateTime)
                throw new BadLogicException("It is not possible to cancel an appointment if there are less than 24 hours left before the start.");
            if(appointment.DateTime < DateTime.Now)
                throw new BadLogicException("It is not possible to cancel an appointment which date and time in the past.");
            appointment.IsReserved = false;
            appointment.PatientId = null;
            appointment.Patient = null;
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
            if (reportDTO.PrescribedMedicines.Count > 0)
            {
                ERecipe recipe = new ERecipe
                {
                    IssuingDate = DateTime.Now,
                    PatientId = appointment.PatientId.Value,
                    Medicines = new List<ERecipeMedicine>()
                };
                foreach (var prescribed in reportDTO.PrescribedMedicines)
                {
                    var medicineInPharmacy = _pharmacyService.ReadMedicine(appointment.PharmacyId, prescribed.MedicineId);
                    if (medicineInPharmacy.InStock < prescribed.Quantity)
                        throw new MissingEntityException($"Pharmacy does not have enough {medicineInPharmacy.Name}.");
                    recipe.Medicines.Add(new ERecipeMedicine
                    {
                        MedicineId = prescribed.MedicineId,
                        Quantity = prescribed.Quantity
                    });
                }
                foreach (var prescribed in reportDTO.PrescribedMedicines)
                    _pharmacyService.ChangeStockFor(appointment.PharmacyId, prescribed.MedicineId, prescribed.Quantity * -1);

                recipe = _eRecipeService.Create(recipe);
                report.ERecipeId = recipe.Id;
            }
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
            if(appointmentDTO.DateTime < DateTime.Now)
            {
                throw new BadLogicException("The given date and time are in the past.");
            }

            var medicalAccount = _accountService.ReadByUserId(appointmentDTO.MedicalStaffId);

            var pharmacist = (Pharmacist)medicalAccount.User;

            if (pharmacist.PharmacyId != appointmentDTO.PharmacyId)
                throw new BadLogicException("Pharmacist must work in that pharmacy.");
                
            ValidateAppointmentDateTime(appointmentDTO, pharmacist.WorkTime, "The given date-time and duration do not overlap with pharmacist's work time.",
                "Pharmacist already has an appointment defined on the given date-time.");

            var patientsAppointments = base.Read().Where(appointment => appointment.PatientId == appointmentDTO.PatientId);
            foreach (var pa in patientsAppointments)
            {
                if (pa.DateTime.Date == appointmentDTO.DateTime.Date && TimeIntervalUtils.TimeIntervalTimesOverlap(
                    pa.DateTime, pa.DateTime.AddMinutes(pa.Duration), appointmentDTO.DateTime,
                    appointmentDTO.DateTime.AddMinutes(appointmentDTO.Duration)))
                    throw new BadLogicException("The given appointment overlaps with the already reserved appointment of the patient.");
            }

            _pharmacyService.TryToRead(appointmentDTO.PharmacyId);

            var priceList = _pharmacyPriceListService.ReadForPharmacy(appointmentDTO.PharmacyId);
            if (priceList == null)
                throw new MissingEntityException("Price list not found for the given pharmacy.");

            var price = appointmentDTO.Price ?? priceList.ExaminationPrice;
            if (price <= 0 || price > 999999)
                throw new BadLogicException("Price must be a valid number between 0 and 999999.");

            var appointmentWithPharmacist = Create(new Appointment
            {
                PharmacyId = appointmentDTO.PharmacyId,
                MedicalStaffId = appointmentDTO.MedicalStaffId,
                DateTime = appointmentDTO.DateTime,
                Duration = appointmentDTO.Duration,
                Price = price,
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
    }
}
