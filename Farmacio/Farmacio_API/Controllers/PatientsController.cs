using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Followings;
using Farmacio_API.Contracts.Responses.Dermatologists;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IERecipeService _eRecipeService;
        private readonly IMapper _mapper;

        public PatientsController(
            IPatientService patientService,
            IMedicalStaffService medicalStaffService,
            IPatientAllergyService patientAllergyService,
            IPatientFollowingsService patientFollowingsService,
            IERecipeService eRecipeService,
            IMapper mapper)
        {
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
            _patientAllergyService = patientAllergyService;
            _patientFollowingsService = patientFollowingsService;
            _eRecipeService = eRecipeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Reads all medical staff's patients.
        /// </summary>
        /// <response code="200">Read patients.</response>
        [HttpGet("my-patients/{medicalAccountId}")]
        public IActionResult GetPatientsForMedicalStaff(Guid medicalAccountId)
        {
            return Ok(_medicalStaffService.ReadPatientsForMedicalStaff(medicalAccountId));
        }

        /// <summary>
        /// Reads medical staff's patients for n-th page.
        /// </summary>
        /// <response code="200">Sort patients.</response>
        [HttpGet("my-patients/{medicalAccountId}/page")]
        public IActionResult GetPatientsForMedicalStaffPageTo(Guid medicalAccountId, [FromQuery] PatientSearchParams searchParams, [FromQuery] PageDTO pageDTO)
        {
            return Ok(_medicalStaffService.ReadPageOfPatientsForMedicalStaffBy(medicalAccountId, searchParams, pageDTO));
        }

        /// <summary>
        /// Searches medical staff's patients.
        /// </summary>
        /// <response code="200">Search patients.</response>
        [HttpGet("my-patients/{medicalAccountId}/search")]
        public IActionResult SearchPatientsForMedicalStaff(Guid medicalAccountId, [FromQuery] PatientSearchParams searchParams)
        {
            return Ok(_medicalStaffService.ReadPatientsForMedicalStaffBy(medicalAccountId, searchParams));
        }

        /// <summary>
        /// Returns patient specified by id.
        /// </summary>
        /// <response code="200">Returns patient.</response>
        /// <response code="404">Unable to return patient because he does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public IActionResult GetPatient(Guid id)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(id))
                                .Authorize();

            return Ok(_patientService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new patient in the system.
        /// </summary>
        /// <response code="200">Created patient.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPost]
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
        [Authorize(Roles = "Patient")]
        [HttpPut]
        public IActionResult UpdatePatient(UpdatePatientRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.Account.Id))
                    .Authorize();

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
        [Authorize(Roles = "Patient")]
        [HttpPost("add-allergies")]
        public IActionResult CreateAllergies(PatientAllergyDTO request)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(AccountSpecific.For(request.patientId))
                .Authorize();
            return Ok(_patientAllergyService.CreateAllergies(request));
        }

        /// <summary>
        /// Returns patients allergies.
        /// </summary>
        /// <response code="200">Patients allergies.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("patients-allergies/{patientId}")]
        public IActionResult GedPatientsAllergies(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(AccountSpecific.For(patientId))
                .Authorize();
            return Ok(_patientAllergyService.GetPatientsAllergies(patientId));
        }

        /// <summary>
        /// Creates a new patient's follow in the system.
        /// </summary>
        /// <response code="200">Returns created patient follow object.</response>
        /// <response code="400">Unable to follow pharmacy because patient already follow it.</response>
        /// <response code="404">Given patient or pharmacy does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("{patientId}/followings")]
        public IActionResult Follow(CreatePatientFollowRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.PatientId))
                    .Authorize();

            return Ok(_patientFollowingsService.Follow(request.PatientId, request.PharmacyId));
        }

        /// <summary>
        /// Reads all followings for specific patient.
        /// </summary>
        /// <response code="200">Returns list of pharmacies that patient follow.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientId}/followings")]
        public IActionResult GetPatientFollowings(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(patientId))
                    .Authorize();

            var patientFollowings = _patientFollowingsService.ReadFollowingsOf(patientId);

            return Ok(patientFollowings.Select(follow => new PatientFollowResponse
            {
                FollowId = follow.Id,
                Since = follow.CreatedAt,
                Pharmacy = follow.Pharmacy
            }));
        }

        /// <summary>
        /// Deletes an existing patient follow from the system.
        /// </summary>
        /// <response code="200">Returns deleted patient follow object.</response>
        /// <response code="404">Given follow does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpDelete("{patientId}/followings/{followId}")]
        public IActionResult Unfollow(Guid followId)
        {
            var follow = _patientFollowingsService.Read(followId);
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(follow.PatientId))
                                .Authorize();

            return Ok(_patientFollowingsService.Unfollow(followId));
        }

        /// <summary>
        /// Deletes an existing patient allergy from the system.
        /// </summary>
        /// <response code="200">Returns deleted patient allergy object.</response>
        /// <response code="404">Given allergy does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpDelete("{patientId}/allergy/{medicineId}")]
        public IActionResult DeleteAllergy(Guid patientId, Guid medicineId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(AccountSpecific.For(patientId))
                .Authorize();
            return Ok(_patientAllergyService.Delete(patientId, medicineId));
        }

        /// <summary>
        /// Returns patient's eRecipes.
        /// </summary>
        /// <response code="200">Returns patient's eRecipes.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientUserId}/eRecipes/sort")]
        public IActionResult SortERecipes(Guid patientUserId, [FromQuery] ERecipesSortFilterParams sortFilterParams)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientUserId))
                .Authorize();
            return Ok(_eRecipeService.SortFor(patientUserId, sortFilterParams));
        }

        /// <summary>
        /// Returns patient's eRecipes for n-th page.
        /// </summary>
        /// <response code="200">Returns patient's eRecipes.</response>
        /// <response code="404">Given patient does not exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientUserId}/eRecipes/sort/page")]
        public IActionResult SortERecipesPageTo(Guid patientUserId, [FromQuery] ERecipesSortFilterParams sortFilterParams, [FromQuery] PageDTO pageDTO)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(UserSpecific.For(patientUserId))
                .Authorize();
            return Ok(_eRecipeService.SortForPageTo(patientUserId, sortFilterParams, pageDTO));
        }
    }
}