using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Followings;
using Farmacio_API.Contracts.Responses.Dermatologists;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        private readonly IPatientFollowingsService _patientFollowingsService;
        private readonly IMapper _mapper;

        public PatientsController(
            IPatientService patientService,
            IMedicalStaffService medicalStaffService,
            IPatientAllergyService patientAllergyService,
            IPatientFollowingsService patientFollowingsService,
            IMapper mapper)
        {
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
            _patientAllergyService = patientAllergyService;
            _patientFollowingsService = patientFollowingsService;
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
        /// Returns patient specified by id.
        /// </summary>
        /// <response code="200">Returns patient.</response>
        /// <response code="404">Unable to return patient because he does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult GetPatient(Guid id)
        {
            return Ok(_patientService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new patient in the system.
        /// </summary>
        /// <response code="200">Created patient.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPost("")]
        public IActionResult CreatePatient(CreatePatientRequest request)
        {
            var patient = _mapper.Map<Account>(request);
            _patientService.Create(patient);

            return Ok(patient);
        }

        /// <summary>
        /// Updates an existing patient from the system.
        /// </summary>
        /// <response code="200">Returns updated patient.</response>
        /// <response code="404">Unable to update patient because he does not exist.</response>
        [HttpPut("")]
        public IActionResult UpdatePatient(UpdatePatientRequest request)
        {
            var patient = _mapper.Map<Account>(request);
            var updatedPatient = _patientService.Update(patient);

            return Ok(updatedPatient);
        }

        /// <summary>
        /// Add patients allergies.
        /// </summary>
        /// <response code="200">Added allergies.</response>
        /// <response code="404">Given medicine does not exist in the system.</response>
        /// <response code="400">Given allergy already exists in the system.</response>
        [HttpPost("add-allergies")]
        public IActionResult CreateAllergies(PatientAllergyDTO request)
        {
            var pharmacy = _mapper.Map<PatientAllergyDTO>(request);
            return Ok(_patientAllergyService.CreateAllergies(request));
        }

        /// <summary>
        /// Returns patients allergies.
        /// </summary>
        /// <response code="200">Patients allergies.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [HttpGet("patients-allergies/{patientId}")]
        public IActionResult GedPatientsAllergies(Guid patientId)
        {
            return Ok(_patientAllergyService.GetPatientsAllergies(patientId));
        }

        /// <summary>
        /// Creates a new patient's follow in the system.
        /// </summary>
        /// <response code="200">Returns created patient follow object.</response>
        /// <response code="400">Unable to follow pharmacy because patient already follow it.</response>
        /// <response code="404">Given patient or pharmacy does not exist in the system.</response>
        [HttpPost("{patientId}/followings")]
        public IActionResult Follow(CreatePatientFollowRequest request)
        {
            return Ok(_patientFollowingsService.Follow(request.PatientId, request.PharmacyId));
        }
    }
}