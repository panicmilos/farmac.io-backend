using System;
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
        
        public AppointmentService(IRepository<Appointment> repository, IDermatologistService dermatologistService
            , IPharmacyService pharmacyService) : base(repository)
        {
            _dermatologistService = dermatologistService;
            _pharmacyService = pharmacyService;
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
            
            var from = appointment.DateTime;
            var to = from.AddHours(appointment.Duration);
            if (!TimeIntervalUtils.TimeIntervalTimesOverlap(from, to, workPlace.WorkTime.From, workPlace.WorkTime.To))
                throw new InvalidAppointmentDateTimeException(
                    "The given date-time and duration do not overlap with dermatologist's work time.");
            
            return Create(new Appointment
            {
                Pharmacy = pharmacy,
                MedicalStaff = dermatologist,
                DateTime = appointment.DateTime,
                Duration = appointment.Duration,
                Price = appointment.Price ?? 50.0f
            });
        }
    }
}