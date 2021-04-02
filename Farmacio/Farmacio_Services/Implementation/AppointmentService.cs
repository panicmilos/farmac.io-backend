using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public AppointmentService(IRepository<Appointment> repository
            , IPharmacyService pharmacyService, IAccountService accountService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , IPharmacyPriceListService pharmacyPriceListService) : base(repository)
        {
            _pharmacyService = pharmacyService;
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _pharmacyPriceListService = pharmacyPriceListService;
        }

        public IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId)
        {
            return Read().Where(a => a.MedicalStaff.Id == medicalStaffId).ToList();
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
                .GetDermatologistWorkPlaceInPharmacy(appointment.MedicalStaffId, pharmacy.Id);
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
    }
}