using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("medical-staff")]
    [Produces("application/json")]
    public class MedicalStaffController : ControllerBase
    {
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IMapper _mapper;

        public MedicalStaffController(IMedicalStaffService medicalStaffService
            , IMapper mapper)
        {
            _medicalStaffService = medicalStaffService;
            _mapper = mapper;
        }

        /// <summary>
        /// Reads all medical staff's patients.
        /// </summary>
        /// <response code="200">Read patients.</response>
        [HttpGet("{id}/patients")]
        public IActionResult GetPatients(Guid id)
        {
            return Ok(_medicalStaffService.GetPatients(id));
        }
    }
}
