﻿using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ReservationService : CrudService<Reservation>, IReservationService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPatientService _patientService;
        private readonly IDiscountService _discountService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IMedicineService _medicineService;

        public ReservationService(
            IPharmacyService pharmacyService,
            IPatientService patientService,
            IDiscountService discountService,
            IEmailDispatcher emailDispatcher,
            ITemplatesProvider templatesProvider,
            IMedicineService medicineService,
            IRepository<Reservation> repository) :
            base(repository)
        {
            _pharmacyService = pharmacyService;
            _patientService = patientService;
            _discountService = discountService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _medicineService = medicineService;
        }

        public IEnumerable<Reservation> ReadFrom(Guid pharmacyId)
        {
            return Read().Where(reservation => reservation.PharmacyId == pharmacyId).ToList();
        }

        public Reservation CancelMedicineReservation(Guid reservationId)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                var reservation = TryToRead(reservationId);

                if (reservation.State == ReservationState.Cancelled)
                {
                    throw new BadLogicException("The reservation has already been canceled.");
                }

                if (reservation.State == ReservationState.Done)
                {
                    throw new BadLogicException("The reservation has already been picked up.");
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


                var updatedReservation = base.Update(reservation);

                transaction.Commit();
                return updatedReservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
            }
        }

        public Reservation CreateReservation(Reservation reservation, bool checkIsMedicineForRecipe)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                _pharmacyService.TryToRead(reservation.PharmacyId);
                var patientAccount = _patientService.ReadByUserId(reservation.PatientId);
                if (patientAccount == null)
                    throw new MissingEntityException("Patient not found.");
                var patient = (Patient)patientAccount.User;

                if (_patientService.HasExceededLimitOfNegativePoints(patient.Id))
                {
                    throw new BadLogicException("You have 3 negative points, so you cannot reserve a medicine.");
                }

                if (DateTime.Now.AddHours(36) > reservation.PickupDeadline)
                {
                    throw new BadLogicException("You have to reserve medicines at least 36 hours before pickup deadline.");
                }

                reservation.UniqueId = GetUniqueId();
                reservation.State = ReservationState.Reserved;

                foreach (var reservedMedicine in reservation.Medicines)
                {
                    var medicine = _medicineService.TryToRead(reservedMedicine.MedicineId);

                    var medicineInPharmacy = _pharmacyService.ReadMedicine(reservation.PharmacyId, reservedMedicine.MedicineId);

                    if (!checkIsMedicineForRecipe)
                    {
                        if (medicine.IsRecipeOnly)
                        {
                            throw new BadLogicException($"The {medicine.Name} solds only with recipe.");
                        }
                    }

                    if (medicineInPharmacy.InStock < reservedMedicine.Quantity)
                        throw new MissingEntityException($"Pharmacy does not have enough {medicineInPharmacy.Name}.");
                    reservedMedicine.Price = DiscountUtils.ApplyDiscount(medicineInPharmacy.Price,
                        _discountService.ReadDiscountFor(reservation.PharmacyId, patient.Id));
                }

                foreach (var reservedMedicine in reservation.Medicines)
                    _pharmacyService.ChangeStockFor(reservation.PharmacyId, reservedMedicine.MedicineId, -reservedMedicine.Quantity);

                var createdReservation = base.Create(reservation);
                var email = _templatesProvider.FromTemplate<Email>("Reservation", new
                {
                    To = patientAccount.Email,
                    Name = patient.FirstName,
                    Id = reservation.UniqueId,
                    Deadline = reservation.PickupDeadline.ToString("dd-MM-yyyy HH:mm")
                });
                _emailDispatcher.Dispatch(email);

                transaction.Commit();
                return createdReservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
            }
        }

        public bool DidPatientReserveMedicine(Guid medicineId, Guid patientId)
        {
            var reservations = Read().ToList().Where(reservation => reservation.PatientId == patientId && reservation.State == ReservationState.Done
                                            && reservation.PickupDeadline < DateTime.Now).ToList();
            reservations = reservations.Where(reservation =>
            {
                var medicines = reservation.Medicines;
                return medicines.FirstOrDefault(medicine => medicine.MedicineId == medicineId) != null;
            }).ToList();
            return reservations.Any();
        }

        public IEnumerable<Reservation> ReadFor(Guid patientId)
        {
            return Read().Where(reservation => reservation.PatientId == patientId).ToList();
        }

        public IEnumerable<SmallReservedMedicineDTO> ReadMedicinesForReservation(Guid reservationId)
        {
            var reservation = TryToRead(reservationId);

            return reservation.Medicines.Select(reservedMedicine => new SmallReservedMedicineDTO
            {
                MedicineId = reservedMedicine.MedicineId,
                Price = reservedMedicine.Price,
                Quantity = reservedMedicine.Quantity
            }).ToList();
        }

        public IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId)
        {
            var patient = _patientService.ReadByUserId(patientId);
            if (patient == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            return Read().ToList().Where(reservation =>
                reservation.State == ReservationState.Reserved && reservation.PatientId == patientId && reservation.PickupDeadline > DateTime.Now)
                .Select(reservation => new SmallReservationDTO
                {
                    ReservationId = reservation.Id,
                    UniqueId = reservation.UniqueId,
                    PickupDeadline = reservation.PickupDeadline,
                    Price = reservation.Medicines.ToList().Sum(medicine => medicine.Quantity * medicine.Price),
                    PharmacyId = reservation.PharmacyId
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

        public void DeleteNotPickedUpReservations()
        {
            foreach (var reservation in Read().ToList())
            {
                if (reservation.State == ReservationState.Reserved && reservation.PickupDeadline < DateTime.Now)
                {
                    foreach (var reservedMedicine in reservation.Medicines.ToList())
                    {
                        _pharmacyService.ReturnMedicinesInStock(reservation.PharmacyId, reservedMedicine.MedicineId, reservedMedicine.Quantity);
                    }

                    reservation.State = ReservationState.Cancelled;
                    base.Update(reservation);

                    var patient = _patientService.ReadByUserId(reservation.PatientId);
                    (patient.User as Patient).NegativePoints++;
                    _patientService.Update(patient);
                }
            }
        }

        public Reservation ReadReservationInPharmacyByUniqueId(string uniqueId, Guid pharmacyId)
        {
            var reservation = Read().FirstOrDefault(reservation => reservation.UniqueId == uniqueId);
            if (reservation == default)
                throw new MissingEntityException($"Reservation with id '{uniqueId}' not found.");
            if (reservation.State != ReservationState.Reserved)
                throw new BadLogicException("The reservation has been either canceled or picked up.");
            if (reservation.PharmacyId != pharmacyId)
                throw new BadLogicException("Reservation was not made in this pharmacy.");
            return reservation;
        }

        public Reservation MarkReservationAsDone(Guid reservationId)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                var reservation = TryToRead(reservationId);
                if (reservation.State == ReservationState.Cancelled)
                    throw new BadLogicException("The reservation has already been canceled.");
                if (reservation.State == ReservationState.Done)
                    throw new BadLogicException("The reservation has already been picked up.");
                if (reservation.PickupDeadline.AddHours(-24) < DateTime.Now)
                    throw new BadLogicException("The reservation is overdue, or less than 24h remained until the pickup deadline.");
                reservation.State = ReservationState.Done;
                base.Update(reservation);
                transaction.Commit();

                var patientAccount = _patientService.TryToRead(reservation.PatientId);
                var email = _templatesProvider.FromTemplate<Email>("ReservationIssued", new
                {
                    To = patientAccount.Email,
                    Name = reservation.Patient.FirstName,
                    Id = reservation.UniqueId
                });
                _emailDispatcher.Dispatch(email);

                return reservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
            }
        }

        public IEnumerable<SmallReservationDTO> ReadPatientPastReservations(Guid patientId)
        {
            var patient = _patientService.ReadByUserId(patientId);
            if (patient == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            return Read().ToList().Where(reservation => reservation.State == ReservationState.Done && reservation.PatientId == patientId)
                .Select(reservation => new SmallReservationDTO
                {
                    ReservationId = reservation.Id,
                    UniqueId = reservation.UniqueId,
                    PickupDeadline = reservation.PickupDeadline,
                    Price = reservation.Medicines.ToList().Sum(medicine => medicine.Quantity * medicine.Price),
                    PharmacyId = reservation.PharmacyId
                });
        }
    }
}