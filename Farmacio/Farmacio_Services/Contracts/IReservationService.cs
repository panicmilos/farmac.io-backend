using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IReservationService : ICrudService<Reservation>
    {
        Reservation CancelMedicineReservation(Guid reservationId);
        IEnumerable<SmallReservationDTO> ReadPatientReservations(Guid patientId);
    }
}