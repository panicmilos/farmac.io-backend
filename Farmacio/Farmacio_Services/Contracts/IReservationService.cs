﻿using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IReservationService : ICrudService<Reservation>
    {
        IEnumerable<Reservation> ReadFor(Guid pharmacyId);
        Reservation CancelMedicineReservation(Guid reservationId);

        IEnumerable<Reservation> ReadFor(Guid patientId);

        IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId);

        IEnumerable<SmallReservedMedicineDTO> ReadMedicinesForReservation(Guid reservationId);
    }
}