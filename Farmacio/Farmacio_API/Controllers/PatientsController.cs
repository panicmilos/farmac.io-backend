using AutoMapper;
using Farmacio_Models.DTO;
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
        private readonly IPatientAllergyService _patientAllergyService;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, IMedicalStaffService medicalStaffService, IPatientAllergyService patientAllergyService
            , IMapper mapper)
        {
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
            _patientAllergyService = patientAllergyService;
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


        /// <summary>
        /// Add patients allergies.
        /// </summary>
        /// <response code="200">Added allergies.</response>
        /// <response code="404">Given medicine does not exist in the system.</response>
        /// <response code="400">Given allergy already exists in the system.</response>
        [HttpPost("allergies")]
        public IActionResult CreateAllergies(PatientAllergyDTO request)
        {
            var pharmacy = _mapper.Map<PatientAllergyDTO>(request);
            return Ok(_patientAllergyService.Create(request));
        }
    }
}
