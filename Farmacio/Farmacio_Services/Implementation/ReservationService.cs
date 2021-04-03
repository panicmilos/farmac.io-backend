using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Repositories.Contracts;
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

        public Reservation CancelMedicineReservation(Guid reservationId)
        {
            var reservation = TryToRead(reservationId);

            if (reservation.State == ReservationState.Cancelled)
            {
                throw new BadLogicException("The reservation has already been canceled.");
            }

            if (reservation.State == ReservationState.Done)
            {
                throw new BadLogicException("The reservation has already been picked up");
            }

            if (DateTime.Now.AddHours(24) > reservation.PickupDeadline)
            {
                throw new BadLogicException("It is not possible to cancel a reservation if there are less than 24 left until pickup medicines.");
            }

            reservation.State = ReservationState.Cancelled;

            foreach (var reservedMedicine in reservation.Medicines)
            {
                _pharmacyService.ChangeStockFor(reservation.PharmacyId, reservedMedicine.MedicineId, reservedMedicine.Quantity);
            }

            return base.Update(reservation);
        }

        public override Reservation Create(Reservation reservation)
        {
            _pharmacyService.TryToRead(reservation.PharmacyId);

            var patientAccount = _patientService.TryToRead(reservation.PatientId);
           
            var patient = (Patient)patientAccount.User;
            if (_patientService.ExceededLimitOfNegativePoints(patient.Id))
            {
                throw new BadLogicException("You have 3 negative points, so you cannot reserve a medicine.");
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

            var createdReservation = Create(reservation);
            var email = _templatesProvider.FromTemplate<Email>("Reservation", new { Name = patientAccount.User.FirstName, Id = reservation.UniqueId, Deadline = reservation.PickupDeadline.ToString("dd-MM-yyyy HH:mm") });
            _emailDispatcher.Dispatch(email);

            return createdReservation;
        }

        public IEnumerable<SmallReservedMedicineDTO> ReadMedicinesForReservation(Guid reservationId)
        {
            var reservation = TryToRead(reservationId);

            return reservation.Medicines.Select(reservedMedicine => new SmallReservedMedicineDTO
            {
                MedicineId = reservedMedicine.MedicineId, Price = reservedMedicine.Price,
                Quantity = reservedMedicine.Quantity
            }).ToList();
        }

        public IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId)
        {
            var reservations = Read().ToList();
            var patientReservations = new List<SmallReservationDTO>();
            foreach(var reservation in reservations)
            {
                if(reservation.State == ReservationState.Reserved && reservation.PatientId == patientId && reservation.CreatedAt < DateTime.Now)
                {
                    var price = reservation.Medicines.ToList().Sum(medicine => medicine.Quantity * medicine.Price);
                    patientReservations.Add(new SmallReservationDTO
                    {
                        ReservationId = reservation.Id,
                        PickupDeadline = reservation.PickupDeadline,
                        Price = price,
                        PharmacyId = reservation.PharmacyId
                    });
                }
            }
            return patientReservations;
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