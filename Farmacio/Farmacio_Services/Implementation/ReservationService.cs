using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ReservationService : CrudService<Reservation>, IReservationService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPatientService _patientService;

        public ReservationService(IPharmacyService pharmacyService, IPatientService patientService, IRepository<Reservation> repository) :
            base(repository)
        {
            _pharmacyService = pharmacyService;
            _patientService = patientService;
        }

        public Reservation CancelMedicineReservation(Guid reservationId)
        {
            var reservation = base.Read(reservationId);
            if (reservation == null)
            {
                throw new MissingEntityException("Given reservation does not exist in the system.");
            }

            if(reservation.State == ReservationState.Cancelled)
            {
                throw new BadLogicException("The reservation has already been canceled.");
            }

            if(reservation.State == ReservationState.Done)
            {
                throw new BadLogicException("The reservation has already been picked up");
            }

            if(DateTime.Now.AddHours(24) > reservation.PickupDeadline)
            {
                throw new BadLogicException("It is not possible to cancel a reservation if there are less than 24 left until pickup medicines.");
            }

            reservation.State = ReservationState.Cancelled;

            foreach(ReservedMedicine reservedMedicine in reservation.Medicines)
            {
                _pharmacyService.ChangeStockFor(reservation.PharmacyId, reservedMedicine.MedicineId, reservedMedicine.Quantity);
            }

            return base.Update(reservation);

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

            return base.Create(reservation);
        }

        public IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId)
        {
            return base.Read()
                .Where(reservation => reservation.State == ReservationState.Reserved && reservation.PatientId == patientId)
                .Select(reservation => new SmallReservationDTO
                {
                    ReservationId = reservation.Id,
                    PickupDeadline = reservation.PickupDeadline
                });
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