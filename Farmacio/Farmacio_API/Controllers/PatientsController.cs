using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("patients")]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, IMedicalStaffService medicalStaffService
            , IMapper mapper)
        {
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
            _mapper = mapper;
        }

        /// <summary>
        /// Reads all medical staff's patients.
        /// </summary>
        /// <response code="200">Read patients.</response>
        [HttpGet("my-patients/{medicalId}")]
        public IActionResult GetPatientsForMedicalStaff(Guid medicalId)
        {
            return Ok(_medicalStaffService.ReadPatientsForMedicalStaff(medicalId));
        }

        /// <summary>
        /// Reads and sorts all medical staff's patients.
        /// </summary>
        /// <response code="200">Sort patients.</response>
        [HttpGet("my-patients/{medicalId}/sort")]
        public IActionResult GetSortedPatientsForMedicalStaff(Guid medicalId, string crit, bool isAsc)
        {
            return Ok(_medicalStaffService.ReadSortedPatientsForMedicalStaff(medicalId, crit, isAsc));
        }

        /// <summary>
        /// Searches medical staff's patients.
        /// </summary>
        /// <response code="200">Search patients.</response>
        [HttpGet("my-patients/{medicalId}/search")]
        public IActionResult SearchPatientsForMedicalStaff(Guid medicalId, string name)
        {
            return Ok(_medicalStaffService.SearchPatientsForMedicalStaff(medicalId, name));
        }
    }
}
