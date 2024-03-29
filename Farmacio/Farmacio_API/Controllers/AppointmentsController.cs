﻿using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Appointments;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [Route("appointments")]
    [ApiController]
    [Produces("application/json")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILoyaltyPointsService _loyaltyPointsService;
        private readonly ILoyaltyProgramService _loyaltyProgramService;
        private readonly IMapper _mapper;

        public AppointmentsController(
            IAppointmentService appointmentService,
            ILoyaltyPointsService loyaltyPointsService,
            ILoyaltyProgramService loyaltyProgramService,
            IMapper mapper)
        {
            _appointmentService = appointmentService;
            _loyaltyPointsService = loyaltyPointsService;
            _loyaltyProgramService = loyaltyProgramService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all appointments from the system.
        /// </summary>
        /// <response code="200">Returns list of appointments.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IEnumerable<Appointment> ReadAppointments()
        {
            return _appointmentService.Read();
        }

        /// <summary>
        /// Returns appointment specified by id.
        /// </summary>
        /// <response code="200">Returns appointment.</response>
        /// <response code="401">Unable to return appointment because it does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult ReadAppointment(Guid id)
        {
            var appointment = _appointmentService.Read(id);
            if (appointment == null)
            {
                throw new MissingEntityException();
            }

            return Ok(appointment);
        }

        /// <summary>
        /// Creates a new appointment in the system.
        /// </summary>
        /// <response code="200">Created appointment.</response>
        /// <response code="404">Pharmacy, dermatologist or dermatologists work place not found.</response>
        /// <response code="400">Invalid date-time and duration.</response>
        [HttpPost("dermatologist")]
        [Authorize(Roles = "PharmacyAdmin")]
        public IActionResult CreateDermatologistAppointment(CreateAppointmentRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(request.PharmacyId))
                .Authorize();
            var appointment = _mapper.Map<CreateAppointmentDTO>(request);
            _appointmentService.CreateDermatologistAppointment(appointment);

            return Ok(appointment);
        }

        /// <summary>
        /// Make an appointment with a dermatologist.
        /// </summary>
        /// <response code="200">Maked appointment.</response>
        /// <response code="404">Appointment not found ot patient not found.</response>
        /// <response code="400">Patient already has appointment in this time, patient has 3 or more negative points, appointment is already reserved.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("make-appointment")]
        public IActionResult MakeAppointmentWithDermatologist(CreateAppointmentWithDermatologist request)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(request.PatientId))
                .Authorize();
            var appointment = _mapper.Map<MakeAppointmentWithDermatologistDTO>(request);
            _appointmentService.MakeAppointmentWithDermatologist(appointment);

            return Ok(appointment);
        }

        /// <summary>
        /// Sort an appointments with a dermatologist in pharmacy.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [HttpGet("sort")]
        public IActionResult SortAppointmentsForDermatologist(Guid pharmacyId, string criteria, bool isAsc)
        {
            var appointments = _appointmentService.ReadForDermatologistsInPharmacy(pharmacyId);
            return Ok(_appointmentService.SortAppointments(appointments, criteria, isAsc));
        }

        /// <summary>
        /// Returns patients history of visits to a dermatologist.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-with-dermatologists/{patientId}")]
        public IActionResult ReadPatientsHistoryOfDermatologistsVisits(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            return Ok(_appointmentService.ReadPatientsHistoryOfVisitingDermatologists(patientId));
        }

        /// <summary>
        /// Sort History of dermatology visits.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-with-dermatologists/{patientId}/sort")]
        public IActionResult SortHistoryOfVisitingDermatologist(Guid patientId, string criteria, bool isAsc)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            var appointments = _appointmentService.ReadPatientsHistoryOfVisitingDermatologists(patientId);
            return Ok(_appointmentService.SortAppointments(appointments, criteria, isAsc));
        }

        /// <summary>
        /// Returns patients future appointments.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("future-appointments/{patientId}")]
        public IEnumerable<Appointment> GetFutureAppointmentsWithDermatologists(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            return _appointmentService.ReadFutureExaminationAppointmentsFor(patientId);
        }

        /// <summary>
        /// Cancel appointment with dermatologist.
        /// </summary>
        /// <response code="200">Returns canceled appointment.</response>
        /// <response code="404">Given appointment does not exist in the system.</response>
        /// <response code="400">Unable to cancel appointment in the past or that starts in less than 24 hours or that is not reserved.</response>
        [Authorize(Roles = "Patient")]
        [HttpDelete("cancel-appointment/{appointmentId}")]
        public IActionResult CancelAppointmentWithDermatologist(Guid appointmentId)
        {
            var appointment = _appointmentService.TryToRead(appointmentId);

            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(appointment.PatientId ?? Guid.Empty))
                .Authorize();

            return Ok(_appointmentService.CancelAppointmentWithDermatologist(appointmentId));
        }

        /// <summary>
        /// Creates an examination or consultation report for appointment.
        /// </summary>
        /// <response code="200">Created report.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="400">Unable to create report for not reserved appointment.</response>
        [Authorize(Roles = "Pharmacist, Dermatologist")]
        [HttpPost("{appointmentId}/report")]
        public IActionResult CreateReport(Guid appointmentId, CreateReportRequest request)
        {
            var appointment = _appointmentService.TryToRead(appointmentId);

            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(appointment.MedicalStaffId))
                                .Authorize();

            var reportDTO = _mapper.Map<CreateReportDTO>(request);
            reportDTO.AppointmentId = appointmentId;

            var createdReport = _appointmentService.CreateReport(reportDTO);

            var patientAccount = _loyaltyPointsService.GivePointsFor(appointment);
            _loyaltyProgramService.UpdateLoyaltyProgramFor(patientAccount);

            return Ok(createdReport);
        }

        /// <summary>
        /// Returns medical staff's appointments that are reserved but not reported for today.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        [Authorize(Roles = "Pharmacist, Dermatologist")]
        [HttpGet("my-appointments/{medicalStaffId}")]
        public IActionResult GetAppointmentsForReportPage(Guid medicalStaffId)
        {
            AuthorizationRuleSet.For(HttpContext)
                               .Rule(UserSpecific.For(medicalStaffId))
                               .Authorize();
            return Ok(_appointmentService.ReadForReportPage(medicalStaffId));
        }

        /// <summary>
        /// Creates a report for appointment, with note that patient did not show up.
        /// </summary>
        /// <response code="200">Created report.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="400">Unable to create report for not reserved appointment.</response>
        [Authorize(Roles = "Pharmacist, Dermatologist")]
        [HttpPost("{appointmentId}/not-show-up")]
        public IActionResult NotePatientDidNotShowUp(Guid appointmentId, CreateReportRequest request)
        {
            var appointment = _appointmentService.TryToRead(appointmentId);

            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(appointment.MedicalStaffId))
                                .Authorize();

            var reportDTO = _mapper.Map<CreateReportDTO>(request);
            reportDTO.AppointmentId = appointmentId;
            return Ok(_appointmentService.NotePatientDidNotShowUp(reportDTO));
        }

        /// <summary>
        /// Returns appointments with pharmacists in future.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("future-with-pharmacists/{patientId}")]
        public IActionResult GetFutureAppointmentsWithPharmacists(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            return Ok(_appointmentService.ReadFutureConsultationAppointmentsFor(patientId));
        }

        /// <summary>
        /// Creates a new appointment in the system.
        /// </summary>
        /// <response code="200">Created appointment.</response>
        /// <response code="400">Invalid date-time and duration.</response>
        /// <response code="404">Something not found.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("pharmacist/as-user")]
        public IActionResult CreatePharmacistAppointmenAsUser(CreateAppointmentRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(request.PatientId ?? Guid.Empty))
                .Authorize();
            var appointment = _mapper.Map<CreateAppointmentDTO>(request);
            appointment.Price = null; // then service method will get price from the pharmacy
            _appointmentService.CreatePharmacistAppointment(appointment);
            return Ok(appointment);
        }

        /// <summary>
        /// Cancel appointment with pharmacist.
        /// </summary>
        /// <response code="200">Returns canceled appointment.</response>
        /// <response code="400">Unable to cancel appointment in the past or that starts in less than 24 hours or that is not reserved.</response>
        /// <response code="404">Given appointment does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpDelete("pharmacist/{appointmentId}")]
        public IActionResult CancelAppointmentWithPharmacist(Guid appointmentId)
        {
            var appointment = _appointmentService.TryToRead(appointmentId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(appointment.PatientId ?? Guid.Empty))
                .Authorize();
            return Ok(_appointmentService.CancelAppointmentWithPharmacist(appointmentId));
        }

        /// <summary>
        /// Creates a new appointment in the system.
        /// </summary>
        /// <response code="200">Created appointment.</response>
        /// <response code="404">Something not found.</response>
        /// <response code="400">Invalid date-time and duration.</response>
        [Authorize(Roles = "Pharmacist, Dermatologist")]
        [HttpPost("another")]
        public IActionResult CreateAnotherAppointment(CreateAppointmentRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(request.MedicalStaffId))
                .Authorize();
            var appointmentDTO = _mapper.Map<CreateAppointmentDTO>(request);
            _appointmentService.CreateAnotherAppointmentByMedicalStaff(appointmentDTO);
            return Ok(appointmentDTO);
        }

        /// <summary>
        /// Returns medical staff's appointments for work calendar.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        [Authorize(Roles = "Pharmacist, Dermatologist")]
        [HttpGet("for-calendar/{medicalStaffId}")]
        public IActionResult GetAppointmentsForCalendar(Guid medicalStaffId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(medicalStaffId))
                .Authorize();
            return Ok(_appointmentService.ReadAppointmentsForCalendar(medicalStaffId));
        }

        /// <summary>
        /// Returns patients history of visits to a pharmacists.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-visit-pharmacists/{patientId}")]
        public IActionResult ReadPatientsHistoryOfVisitngPharmacists(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            return Ok(_appointmentService.ReadPatientsHistoryOfVisitingPharmacists(patientId));
        }

        /// <summary>
        /// Sort history of pharmacists visits.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-visit-pharmacists/{patientId}/sort")]
        public IActionResult SortHistoryOfVisitingPharmacists(Guid patientId, string criteria, bool isAsc)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();
            var appointments = _appointmentService.ReadPatientsHistoryOfVisitingPharmacists(patientId);
            return Ok(_appointmentService.SortAppointments(appointments, criteria, isAsc));
        }

        /// <summary>
        /// Returns patients history of visits to a pharmacists for n-th page.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-visit-pharmacists/{patientId}/page-to")]
        public IActionResult ReadPatientsHistoryOfVisitngPharmacistsByPageTo(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            return Ok(_appointmentService.ReadPageOfPatientHistoryVisitingPharmacists(patientId, pageDTO));
        }

        /// <summary>
        /// Sort history of pharmacists visits for n-th page.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-visit-pharmacists/{patientId}/sort/page-to")]
        public IActionResult SortHistoryOfVisitingPharmacistsByPageTo(Guid patientId, string criteria, bool isAsc, [FromQuery] PageDTO pageDTO)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            var appointments = _appointmentService.ReadPatientsHistoryOfVisitingPharmacists(patientId);
            return Ok(_appointmentService.SortAppointmentsPageTo(appointments, criteria, isAsc, pageDTO));
        }

        /// <summary>
        /// Returns patients history of visits to a dermatologist for n-th page.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-with-dermatologists/{patientId}/page-to")]
        public IActionResult ReadPatientsHistoryOfDermatologistsVisitsByPageTo(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            return Ok(_appointmentService.ReadPageOfPatientHistoryVisitingDermatologists(patientId, pageDTO));
        }

        /// <summary>
        /// Sort history of dermatology visits for n-th page.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("history-with-dermatologists/{patientId}/sort/page-to")]
        public IActionResult SortHistoryOfVisitingDermatologistByPagesTo(Guid patientId, string criteria, bool isAsc, int number, int size)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientId))
                .Authorize();

            var appointments = _appointmentService.ReadPatientsHistoryOfVisitingDermatologists(patientId);
            return Ok(_appointmentService.SortAppointmentsPageTo(appointments, criteria, isAsc, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }
    }
}