using AutoMapper;
using Farmacio_API.Contracts.Requests.Pharmacies;
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
    [Route("appointment")]
    [ApiController]
    [Produces("application/json")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
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

    }
}