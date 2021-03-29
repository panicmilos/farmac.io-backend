using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ReservationService : CrudService<Reservation>, IReservationService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPatientService _patientService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;

        public ReservationService(IPharmacyService pharmacyService, IPatientService patientService, IEmailDispatcher emailDispatcher,
            ITemplatesProvider templatesProvider, IRepository<Reservation> repository) :
            base(repository)
        {
            _pharmacyService = pharmacyService;
            _patientService = patientService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
        }

        public override Reservation Create(Reservation reservation)
        {
            if (_pharmacyService.Read(reservation.PharmacyId) == null)
            {
                throw new MissingEntityException("Given pharmacy does not exist in the system.");
            }

            var patientAccount = _patientService.Read(reservation.PatientId);
            if (_patientService.Read(reservation.PatientId) == null)
            {
                throw new MissingEntityException("Given patient does not exist in the system.");
            }
            reservation.PatientId = patientAccount.User.Id;

            if (DateTime.Now.AddHours(36) > reservation.PickupDeadline)
            {
                throw new BadLogicException("You have to reserve medicines at least 36 hours before pickup deadline.");
            }

            reservation.UniqueId = GetUniqueId();
            reservation.State = ReservationState.Reserved;

            foreach (var reservedMedicine in reservation.Medicines)
            {
                var medicineInPharmacy = _pharmacyService.ReadMedicine(reservation.PharmacyId, reservedMedicine.MedicineId);
                if (medicineInPharmacy.InStock < reservedMedicine.Quantity)
                {
                    throw new MissingEntityException($"Pharmacy does not have enough {medicineInPharmacy.Name}.");
                }
                reservedMedicine.Price = medicineInPharmacy.Price;
                _pharmacyService.ChangeStockFor(reservation.PharmacyId, reservedMedicine.MedicineId, reservedMedicine.Quantity * -1);
            }
            
            var createdReservation = base.Create(reservation);
            var email = _templatesProvider.FromTemplate<Email>("Reservation", new { Name = patientAccount.User.FirstName, Id = reservation.UniqueId, Deadline = reservation.PickupDeadline.ToString("dd-MM-yyyy HH:mm") });
            _emailDispatcher.Dispatch(email);

            return createdReservation;
        }

        private string GetUniqueId()
        {
            string uniqueId;
            do
            {
                uniqueId = StringUtils.RandomString(10);
            } while (!IsIdUnique(uniqueId));

            return uniqueId;
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(reservation => reservation.UniqueId == id) == default;
        }
    }
}