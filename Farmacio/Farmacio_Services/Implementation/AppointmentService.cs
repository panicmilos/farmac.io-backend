using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class AppointmentService : CrudService<Appointment>, IAppointmentService
    {
        private readonly IDermatologistService _dermatologistService;
        private readonly IPharmacyService _pharmacyService;
        private readonly IAccountService _accountService;
        
        public AppointmentService(IRepository<Appointment> repository, IDermatologistService dermatologistService
            , IPharmacyService pharmacyService, IAccountService accountService) : base(repository)
        {
            _dermatologistService = dermatologistService;
            _pharmacyService = pharmacyService;
            _accountService = accountService;
        }

        public IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId)
        {
            return Read().Where(a => a.MedicalStaff.Id == medicalStaffId).ToList();
        }
        
        public IEnumerable<Appointment> ReadForDermatologistsInPharmacy(Guid pharmacyId)
        {
            return Read()
                .ToList()
                .Where(a => a.Pharmacy.Id == pharmacyId &&
                            _accountService.Read().ToList()
                                .FirstOrDefault(acc => acc.User.Id == a.MedicalStaff.Id)?.Role == Role.Dermatologist)
                .ToList();
        }

        public Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointment)
        {
            var pharmacy = _pharmacyService.Read(appointment.PharmacyId);
            if (pharmacy == null)
                throw new MissingEntityException("Pharmacy not found.");
            
            var dermatologistAccount = _dermatologistService.ReadForPharmacy(pharmacy.Id, appointment.MedicalStaffId);
            if (dermatologistAccount == null)
                throw new MissingEntityException("Dermatologist not found inside the given pharmacy.");
            
            var dermatologist = (Dermatologist) dermatologistAccount.User;
            
            var workPlace = dermatologist.WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacy.Id);
            if (workPlace == null)
                throw new MissingEntityException("Dermatologist work place for the given pharmacy id not found.");
            
            ValidateAppointmentDateTime(appointment, workPlace, dermatologist);

            var price = appointment.Price ?? pharmacy.PriceList.ExaminationPrice;
            if(price <= 0 || price > 999999)
                throw new BadLogicException("Price must be a valid number between 0 and 999999.");

            return Create(new Appointment
            {
                Pharmacy = pharmacy,
                MedicalStaff = dermatologist,
                DateTime = appointment.DateTime,
                Duration = appointment.Duration,
                Price = price
            });
        }

        private void ValidateAppointmentDateTime(CreateAppointmentDTO appointment, DermatologistWorkPlace workPlace,
            Dermatologist dermatologist)
        {
            var from = appointment.DateTime;
            var to = @from.AddMinutes(appointment.Duration);
            if (!TimeIntervalUtils.TimeIntervalTimesOverlap(@from, to, workPlace.WorkTime.From, workPlace.WorkTime.To))
                throw new InvalidAppointmentDateTimeException(
                    "The given date-time and duration do not overlap with dermatologist's work time.");

            var overlap = ReadForMedicalStaff(dermatologist.Id).FirstOrDefault(a =>
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