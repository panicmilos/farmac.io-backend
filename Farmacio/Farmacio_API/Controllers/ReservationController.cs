using AutoMapper;
using Farmacio_API.Contracts.Requests.Reservations;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("reservations")]
    [Produces("application/json")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILoyaltyPointsService _loyaltyPointsService;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationService reservationService, ILoyaltyPointsService loyaltyPointsService, IMapper mapper)
        {
            _reservationService = reservationService;
            _loyaltyPointsService = loyaltyPointsService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateReservation(CreateReservationRequest request)
        {
            var reservation = _mapper.Map<Reservation>(request);
            _reservationService.CreateReservation(reservation, false);

            return Ok();
        }

        [HttpGet("futureReservations/{patientId}")]
        public IEnumerable<SmallReservationDTO> GetPatientsFutureReservations(Guid patientId)
        {
            return _reservationService.ReadPatientReservations(patientId);
        }

        [HttpDelete("cancel/{reservationId}")]
        public IActionResult CancelMedicineReservation(Guid reservationId)
        {
            _reservationService.CancelMedicineReservation(reservationId);
            return Ok();
        }

        [HttpGet("reservedMedicines/{reservationId}")]
        public IEnumerable<SmallReservedMedicineDTO> GetMedicinesForReservation(Guid reservationId)
        {
            return _reservationService.ReadMedicinesForReservation(reservationId);
        }

        [HttpGet("in-pharmacy/{pharmacyId}/by-uniqueid/{uniqueId}")]
        public IActionResult GetReservationInPharmacyByUniqueId(string uniqueId, Guid pharmacyId)
        {
            return Ok(_reservationService.ReadReservationInPharmacyByUniqueId(uniqueId, pharmacyId));
        }

        [HttpPut("issue-medicines/{reservationId}")]
        public IActionResult MarkReservationAsDone(Guid reservationId)
        {
            var issuedReservation = _reservationService.MarkReservationAsDone(reservationId);
            _loyaltyPointsService.GivePointsFor(issuedReservation);

            return Ok();
        }
    }
}