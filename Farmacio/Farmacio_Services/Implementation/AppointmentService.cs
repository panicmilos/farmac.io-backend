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

        public AppointmentService(IRepository<Appointment> repository
            , IPharmacyService pharmacyService, IAccountService accountService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , IPharmacyPriceListService pharmacyPriceListService
            , IPatientService patientService
            , IEmailDispatcher emailDispatcher
            , ITemplatesProvider templateProvider) : base(repository)

        {
            _pharmacyService = pharmacyService;
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _pharmacyPriceListService = pharmacyPriceListService;
            _patientService = patientService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templateProvider;
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
            
            ValidateAppointmentDateTime(appointment, workPlace, appointment.MedicalStaffId);

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

        private void ValidateAppointmentDateTime(CreateAppointmentDTO appointment, DermatologistWorkPlace workPlace,
            Guid medicalStaffId)
        {
            var from = appointment.DateTime;
            var to = @from.AddMinutes(appointment.Duration);
            if (!TimeIntervalUtils.TimeIntervalTimesOverlap(@from, to, workPlace.WorkTime.From, workPlace.WorkTime.To))
                throw new InvalidAppointmentDateTimeException(
                    "The given date-time and duration do not overlap with dermatologist's work time.");

            var overlap = ReadForMedicalStaff(medicalStaffId).FirstOrDefault(a =>
            {
                var existingFrom = a.DateTime;
                var existingTo = a.DateTime.AddMinutes(a.Duration);

                var datesAreEqual = existingFrom.Date == @from.Date;
                return datesAreEqual &&
                       TimeIntervalUtils.TimeIntervalTimesOverlap(@from, to, existingFrom, existingTo);
            });
            if (overlap != null)
                throw new InvalidAppointmentDateTimeException(
                    "Dermatologist already has an appointment defined on the given date-time.");
        }

        public Appointment MakeAppointmentWithDermatologist(MakeAppointmentWithDermatologistDTO appointmentRequest)
        {
            var appointmentWithDermatologist = base.Read(appointmentRequest.AppointmentId);
            if(appointmentWithDermatologist == null)
            {
                throw new MissingEntityException("The given appointment does not exist in the system.");
            }

            if(_patientService.Read().Where(account => account.UserId == appointmentRequest.PatientId) == null)
            {
                throw new MissingEntityException("The given patient does not exixst in the system.");
            }

            if (appointmentWithDermatologist.IsReserved)
            {
                throw new BadLogicException("The given appointment is already reserved.");
            }

            if (_patientService.HasExceededLimitOfNegativePoints(appointmentRequest.PatientId))
            {
                throw new BadLogicException("The given patient has 3 or more negative points.");
            }

            if(appointmentWithDermatologist.DateTime < DateTime.Now)
            {
                throw new BadLogicException("The given appointment is in the past.");
            }

            var patientsAppointments = base.Read().Where(appointment => appointment.PatientId == appointmentRequest.PatientId);
            foreach(var appointment in patientsAppointments)
            {
                if(appointment.DateTime.Date == appointmentWithDermatologist.DateTime.Date && TimeIntervalUtils.TimeIntervalTimesOverlap(appointment.DateTime, 
                    appointment.DateTime.AddMinutes(appointment.Duration), appointmentWithDermatologist.DateTime,
                    appointmentWithDermatologist.DateTime.AddMinutes(appointmentWithDermatologist.Duration)))
                {
                    throw new BadLogicException("The given appointment overlaps with the already reserved appointment of the patient.");
                }
            }

            appointmentWithDermatologist.IsReserved = true;
            appointmentWithDermatologist.PatientId = appointmentRequest.PatientId;
            var email = _templatesProvider.FromTemplate<Email>("Appointment", new { Name = appointmentWithDermatologist.Patient.FirstName, Date = appointmentWithDermatologist.DateTime.ToString("dd-MM-yyyy HH:mm") });
            _emailDispatcher.Dispatch(email);
            return base.Update(appointmentWithDermatologist);
        }

        public IEnumerable<Appointment> SortAppointments(string criteria, bool isAsc, IEnumerable<Appointment> appointments)
        {
            var sortingCriteria = new Dictionary<string, Func<Appointment, object>>()
            {
                { "grade", a => a.MedicalStaff.AverageGrade },
                { "price", a => a.Price },
                { "duration", a => a.Duration }
            };

            if (sortingCriteria.TryGetValue(criteria ?? "", out var sortingCriterion)) {
                appointments = isAsc ? appointments.OrderBy(sortingCriterion) : appointments.OrderByDescending(sortingCriterion);
            }

            return appointments;
        }

        public IEnumerable<Appointment> ReadForPatients(Guid patientId)
        {

            if (_patientService.Read().Where(account => account.UserId == patientId) == null)
            {
                throw new MissingEntityException("The given patient does not exixst in the system.");
            }
            return Read().ToList().Where(appointment => appointment.PatientId == patientId && appointment.IsReserved && appointment.DateTime > DateTime.Now);
        }

        public Appointment CancelAppointmentWithDermatologist(Guid appointmentId)
        {
            var appointment = base.TryToRead(appointmentId);
            if(appointment.IsReserved == false)
            {
                throw new BadLogicException("Given appointment is not reserved.");
            }
            if(DateTime.Now.AddHours(24) > appointment.DateTime)
            {
                throw new BadLogicException("It is not possible to cancel an appointment if there are less than 24 hours left before the start.");
            }
            if(appointment.DateTime < DateTime.Now)
            {
                throw new BadLogicException("It is not possible to cancel an appointment which date and time in the past.");
            }
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
                throw new MissingEntityException("The given patient does not exixst in the system.");
            }
            return Read().ToList().Where(appointmet => appointmet.PatientId == patientId && appointmet.IsReserved && appointmet.DateTime < DateTime.Now);
        }
    }
}