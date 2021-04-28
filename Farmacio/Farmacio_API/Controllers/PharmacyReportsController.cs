using AutoMapper;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Farmacio_API.Contracts.Requests.Appointments;
using Farmacio_API.Contracts.Requests.PharmacyReports;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PharmacyReportsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public PharmacyReportsController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Generates a pharmacy examinations reports based on the given time interval.
        /// </summary>
        /// <response code="200">Generated report.</response>
        [HttpPost("pharmacy/{pharmacyId}/examination-report")]
        public IActionResult PharmacyExaminationReport(Guid pharmacyId, TimePeriodRequest timePeriodRequest)
        {
            var timePeriodDto = _mapper.Map<TimePeriodDTO>(timePeriodRequest);
            return Ok(_appointmentService.GenerateExaminationsReportFor(pharmacyId, timePeriodDto));
        }
        
    }
}