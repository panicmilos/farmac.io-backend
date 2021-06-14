using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Reservations;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILoyaltyProgramService _loyaltyProgramService;
        private readonly IMapper _mapper;

        public ReservationsController(
            IReservationService reservationService,
            ILoyaltyPointsService loyaltyPointsService,
            ILoyaltyProgramService loyaltyProgramService,
            IMapper mapper)
        {
            _reservationService = reservationService;
            _loyaltyPointsService = loyaltyPointsService;
            _loyaltyProgramService = loyaltyProgramService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public IActionResult CreateReservation(CreateReservationRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(UserSpecific.For(request.PatientId))
                    .Authorize();

            var reservation = _mapper.Map<Reservation>(request);
            _reservationService.CreateReservation(reservation, false);

            return Ok();
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("futureReservations/{patientId}")]
        public IEnumerable<SmallReservationDTO> GetPatientsFutureReservations(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            return _reservationService.ReadPatientReservations(patientId);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("pastReservations/{patientId}")]
        public IEnumerable<SmallReservationDTO> GetPatientsPastReservations(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            return _reservationService.ReadPatientPastReservations(patientId);
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("cancel/{reservationId}")]
        public IActionResult CancelMedicineReservation(Guid reservationId)
        {
            var reservation = _reservationService.TryToRead(reservationId);

            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(reservation.PatientId))
                .Authorize();

            _reservationService.CancelMedicineReservation(reservationId);
            return Ok();
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("reservedMedicines/{reservationId}")]
        public IEnumerable<SmallReservedMedicineDTO> GetMedicinesForReservation(Guid reservationId)
        {
            var reservation = _reservationService.TryToRead(reservationId);

            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(reservation.PatientId))
                .Authorize();

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
            var patientAccount = _loyaltyPointsService.GivePointsFor(issuedReservation);
            _loyaltyProgramService.UpdateLoyaltyProgramFor(patientAccount);

            return Ok();
        }
    }
}