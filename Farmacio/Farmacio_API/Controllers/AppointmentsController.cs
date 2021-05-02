using AutoMapper;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.Appointments;

namespace Farmacio_API.Controllers
{
    [Route("appointments")]
    [ApiController]
    [Produces("application/json")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPharmacyService _pharmacyService;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentService appointmentService, IPharmacyService pharmacyService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pharmacies from the system.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet]
        public IEnumerable<Appointment> ReadAppointments()
        {
            return _appointmentService.Read();
        }

        /// <summary>
        /// Returns pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns pharmacy.</response>
        /// <response code="401">Unable to return pharmacy because it does not exist in the system.</response>
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
        public IActionResult CreateDermatologistAppointment(CreateAppointmentRequest request)
        {
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
        [HttpPost("make-appointment")]
        public IActionResult MakeAppointmentWithDermatologist(CreateAppointmentWithDermatologist request)
        {
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
        [HttpGet("history/{patientId}")]
        public IActionResult ReadPatientsHistoryOfDermatologistsVisits(Guid patientId)
        {
            return Ok(_appointmentService.ReadPatientsHistoryOfVisitsToDermatologist(patientId));
        }
        
        /// <summary>
        /// Sort History of dermatology visits.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [HttpGet("history/{patientId}/sort")]
        public IActionResult SortHistoryOfVisitingDermatologist(Guid patientId, string criteria, bool isAsc)
        {
            var appointments = _appointmentService.ReadPatientsHistoryOfVisitsToDermatologist(patientId);
            return Ok(_appointmentService.SortAppointments(appointments, criteria, isAsc));
        }

        /// <summary>
        /// Returns patients future appointments.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [HttpGet("future-appointments/{patientId}")]
        public IEnumerable<Appointment> ReadPatientsFutureAppointments(Guid patientId)
        {
            return _appointmentService.ReadForPatients(patientId);
        }

        /// <summary>
        /// Cancel appointment with dermatologist.
        /// </summary>
        /// <response code="200">Returns canceled appointment.</response>
        /// <response code="404">Given appointment does not exist in the system.</response>
        /// <response code="400">Unable to cancel appointment in the past or that starts in less than 24 hours or that is not reserved.</response>
        [HttpDelete("cancel-appointment/{appointmentId}")]
        public IActionResult CancelAppointmentWithDermatologist(Guid appointmentId)
        {
            return Ok(_appointmentService.CancelAppointmentWithDermatologist(appointmentId));
        }

        /// <summary>
        /// Creates an examination or consultation report for appointment.
        /// </summary>
        /// <response code="200">Created report.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="400">Unable to create report for not reserved appointment.</response>
        [HttpPost("{appointmentId}/report")]
        public IActionResult CreateReport(Guid appointmentId, CreateReportRequest request)
        {
            var reportDTO = _mapper.Map<CreateReportDTO>(request);
            reportDTO.AppointmentId = appointmentId;
            return Ok(_appointmentService.CreateReport(reportDTO));
        }

        /// <summary>
        /// Returns medical staff's appointments that are reserved but not reported.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        [HttpGet("my-appointments/{medicalStaffId}")]
        public IActionResult GetAppointmentsForMedicalStaff(Guid medicalStaffId)
        {
            return Ok(_appointmentService.ReadReservedButUnreportedForMedicalStaff(medicalStaffId));
        }

        /// <summary>
        /// Creates a report for appointment, with note that patient did not show up.
        /// </summary>
        /// <response code="200">Created report.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="400">Unable to create report for not reserved appointment.</response>
        [HttpPost("{appointmentId}/not-show-up")]
        public IActionResult NotePatientDidNotShowUp(Guid appointmentId, CreateReportRequest request)
        {
            var reportDTO = _mapper.Map<CreateReportDTO>(request);
            reportDTO.AppointmentId = appointmentId;
            return Ok(_appointmentService.NotePatientDidNotShowUp(reportDTO));
        }

        /// <summary>
        /// Returns appointments with pharmacists in future.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [HttpGet("future-with-pharmacists/{patientId}")]
        public IActionResult GetFutureAppointmentsWithPharmacists(Guid patientId)
        {
            return Ok(_appointmentService.ReadFuturePharmacistsAppointmentsFor(patientId));
        }

        /// <summary>
        /// Creates a new appointment in the system.
        /// </summary>
        /// <response code="200">Created appointment.</response>
        /// <response code="400">Invalid date-time and duration.</response>
        /// <response code="404">Something not found.</response>
        [HttpPost("pharmacist/as-user")]
        public IActionResult CreatePharmacistAppointmenAsUser(CreateAppointmentRequest request)
        {
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
        [HttpDelete("pharmacist/{appointmentId}")]
        public IActionResult CancelAppointmentWithPharmacist(Guid appointmentId)
        {
            return Ok(_appointmentService.CancelAppointmentWithPharmacist(appointmentId));
        }

        /// <summary>
        /// Creates a new appointment in the system.
        /// </summary>
        /// <response code="200">Created appointment.</response>
        /// <response code="404">Something not found.</response>
        /// <response code="400">Invalid date-time and duration.</response>
        [HttpPost("another")]
        public IActionResult CreateAnotherAppointment(CreateAppointmentRequest request)
        {
            var appointment = _mapper.Map<CreateAppointmentDTO>(request);
            _appointmentService.CreateAnotherAppointmentByMedicalStaff(appointment);
            return Ok(appointment);
        }

        /// <summary>
        /// Returns medical staff's appointments for work calendar.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        [HttpGet("for-calendar/{medicalStaffId}")]
        public IActionResult GetAppointmentsForCalendar(Guid medicalStaffId)
        {
            return Ok(_appointmentService.ReadAppointmentsForCalendar(medicalStaffId));
        }

        /// <summary>
        /// Returns patients history of visits to a dermatologist.
        /// </summary>
        /// <response code="200">Returns appointments.</response>
        /// <response code="404">Unable to return appointments because given patient does not exist in the system.</response>
        [HttpGet("history-visit-pharmacists/{patientId}")]
        public IActionResult ReadPatientsHistoryOfVisingPharmacists(Guid patientId)
        {
            return Ok(_appointmentService.ReadPatientsHistoryOfVisitingPharmacists(patientId));
        }

        /// <summary>
        /// Sort History of dermatology visits.
        /// </summary>
        /// <response code="200">Sorted appointments.</response>
        [HttpGet("history-visit-pharmacists/{patientId}/sort")]
        public IActionResult SortHistoryOfVisitingPharmacists(Guid patientId, string criteria, bool isAsc)
        {
            var appointments = _appointmentService.ReadPatientsHistoryOfVisitingPharmacists(patientId);
            return Ok(_appointmentService.SortAppointments(appointments, criteria, isAsc));
        }
    }
}