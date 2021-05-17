using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
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
    [Route("dermatologists")]
    [Produces("application/json")]
    public class DermatologistsController : ControllerBase
    {
        private readonly IDermatologistService _dermatologistService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IMapper _mapper;

        public DermatologistsController(IDermatologistService dermatologistService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , IMapper mapper)
        {
            _dermatologistService = dermatologistService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Reads all existing dermatologists in the system.
        /// </summary>
        /// <response code="200">Read dermatologists.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet]
        public IActionResult GetDermatologists()
        {
            return Ok(_dermatologistService.Read());
        }

        /// <summary>
        /// Search all existing dermatologists in the system.
        /// </summary>
        /// <response code="200">Searched dermatologists.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet("search")]
        public IActionResult SearchDermatologists(string name)
        {
            return Ok(_dermatologistService.SearchByName(name));
        }

        /// <summary>
        /// Returns dermatologists that match the given params from the system.
        /// </summary>
        /// <response code="200">List of dermatologists.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet("filter")]
        public IActionResult FilterDermatologists([FromQuery] MedicalStaffFilterParamsDTO filterParams)
        {
            return Ok(_dermatologistService.ReadBy(filterParams));
        }

        /// <summary>
        /// Reads all existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Read dermatologists with their work places.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet("with-work-places")]
        public IActionResult GetDermatologistsWithWorkPlaces()
        {
            return Ok(_dermatologistService.Read().Select(dermatologistAccount => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = dermatologistAccount,
                WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
            }));
        }

        /// <summary>
        /// Search all existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Searched dermatologists with their work places.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet("with-work-places/search")]
        public IActionResult SearchDermatologistsWithWorkPlaces(string name)
        {
            return Ok(_dermatologistService.SearchByName(name).Select(dermatologistAccount => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = dermatologistAccount,
                WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
            }));
        }

        /// <summary>
        /// Filter all existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Filtered dermatologists with their work places.</response>
        [Authorize(Roles = "Patient, SystemAdmin, PharmacyAdmin")]
        [HttpGet("with-work-places/filter")]
        public IActionResult FilterDermatologistsWithWorkPlaces([FromQuery] MedicalStaffFilterParamsDTO filterParams)
        {
            return Ok(_dermatologistService.ReadBy(filterParams).Select(dermatologistAccount => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = dermatologistAccount,
                WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
            }));
        }
        
        /// <summary>
        /// Filter page of existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Filtered page of dermatologists with their work places.</response>
        [HttpGet("with-work-places/filter/page")]
        public IActionResult FilterDermatologistsWithWorkPlacesPage([FromQuery] MedicalStaffFilterParamsDTO filterParams,
            [FromQuery] PageDTO pageDto)
        {
            return Ok(_dermatologistService.ReadPageBy(filterParams, pageDto).Select(dermatologistAccount => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = dermatologistAccount,
                WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
            }));
        }

        /// <summary>
        /// Reads an existing dermatologist in the system.
        /// </summary>
        /// <response code="200">Read dermatologist.</response>
        [Authorize(Roles = "Dermatologist, SystemAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetDermatologist(Guid id)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(id))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            return Ok(_dermatologistService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new dermatologist in the system.
        /// </summary>
        /// <response code="200">Returns created dermatologist.</response>
        /// <response code="401">Unable to create dermatologist because username or email is already taken.</response>
        [Authorize(Roles = "SystemAdmin, PharmacyAdmin")]
        [HttpPost]
        public IActionResult CreateDermatologist(CreateDermatologistRequest request)
        {
            var dermatologist = _mapper.Map<Account>(request);
            _dermatologistService.Create(dermatologist);

            return Ok(dermatologist);
        }

        /// <summary>
        /// Updates an existing dermatologist from the system.
        /// </summary>
        /// <response code="200">Returns updated dermatologist.</response>
        /// <response code="404">Unable to update dermatologist because he does not exist.</response>
        [Authorize(Roles = "Dermatologist, SystemAdmin")]
        [HttpPut]
        public IActionResult UpdateDermatologist(UpdateDermatologistRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(request.Account.Id))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            var dermatologist = _mapper.Map<Account>(request);
            var updatedDermatologist = _dermatologistService.Update(dermatologist);

            return Ok(updatedDermatologist);
        }

        /// <summary>
        /// Deletes dermatologist from the system.
        /// </summary>
        /// <response code="200">Returns deleted dermatologist.</response>
        /// <response code="404">Unable to delete dermatologist because he does not exist.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteDermatologist(Guid id)
        {
            var deletedDermatologist = _dermatologistService.Delete(id);

            return Ok(deletedDermatologist);
        }

        /// <summary>
        /// Reads pharmacy names where dermatologist works.
        /// </summary>
        /// <response code="200">Pharmacy names.</response>
        [HttpGet("{dermatologistId}/work-place-names")]
        public IActionResult GetDermatologistsWorkPlaceNames(Guid dermatologistId)
        {
            return Ok(_dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistId).Select(dwp => dwp.Pharmacy.Name));
        }
    }
}