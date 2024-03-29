﻿using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IReservationService : ICrudService<Reservation>
    {
        IEnumerable<Reservation> ReadFrom(Guid pharmacyId);

        Reservation CancelMedicineReservation(Guid reservationId);

        IEnumerable<Reservation> ReadFor(Guid patientId);

        IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId);

        IEnumerable<SmallReservedMedicineDTO> ReadMedicinesForReservation(Guid reservationId);

        bool DidPatientReserveMedicine(Guid medicineId, Guid patientId);

        void DeleteNotPickedUpReservations();

        Reservation ReadReservationInPharmacyByUniqueId(string uniqueId, Guid pharmacyId);

        Reservation MarkReservationAsDone(Guid reservationId);

        Reservation CreateReservation(Reservation reservation, bool checkIsMedicineForRecipe);

        IEnumerable<SmallReservationDTO> ReadPatientPastReservations(Guid patientId);
    }
}