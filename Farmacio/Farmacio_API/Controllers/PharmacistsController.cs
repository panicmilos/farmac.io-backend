using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("pharmacists")]
    [Produces("application/json")]
    public class PharmacistsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPharmacistService _pharmacistService;

        public PharmacistsController(IPharmacistService pharmacistService, IMapper mapper)
        {
            _pharmacistService = pharmacistService;
            _mapper = mapper;
        }

        /// <summary>
        /// Reads all existing pharmacists in the system.
        /// </summary>
        /// <response code="200">Read pharmacists.</response>
        [Authorize(Roles = "Patient, SystemAdmin")]
        [HttpGet]
        public IActionResult GetPharmacists()
        {
            return Ok(_pharmacistService.Read());
        }

        /// <summary>
        /// Search all existing pharmacists in the system.
        /// </summary>
        /// <response code="200">Searched pharmacists.</response>
        [Authorize(Roles = "Patient, SystemAdmin")]
        [HttpGet("search")]
        public IActionResult SearchPharmacists(string name)
        {
            return Ok(_pharmacistService.SearchByName(name));
        }

        /// <summary>
        /// Returns pharmacists that match the given params from the system.
        /// </summary>
        /// <response code="200">List of pharmacists.</response>
        [Authorize(Roles = "Patient, SystemAdmin")]
        [HttpGet("filter")]
        public IActionResult FilterPharmacists([FromQuery] MedicalStaffFilterParamsDTO filterParams)
        {
            return Ok(_pharmacistService.ReadBy(filterParams));
        }
        
        /// <summary>
        /// Returns pharmacists page that match the given params from the system.
        /// </summary>
        /// <response code="200">List of pharmacists page.</response>
        [Authorize(Roles = "Patient, SystemAdmin")]
        [HttpGet("filter/page")]
        public IActionResult FilterPharmacistsPage([FromQuery] MedicalStaffFilterParamsDTO filterParams,
            [FromQuery] PageDTO pageDto)
        {
            return Ok(_pharmacistService.ReadPageBy(filterParams, pageDto));
        }

        /// <summary>
        /// Reads an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Read pharmacist.</response>
        [Authorize(Roles = "Pharmacist, SystemAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetPharmacist(Guid id)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(id))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            return Ok(_pharmacistService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new pharmacist in the system.
        /// </summary>
        /// <response code="200">Created pharmacist.</response>
        /// <response code="404">Pharmacy not found.</response>
        /// <response code="401">Username or email is already taken.</response>
        [Authorize(Roles = "SystemAdmin, PharmacyAdmin")]
        [HttpPost]
        public IActionResult CreatePharmacist(CreatePharmacistRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(IsPharmacyAdmin.Of(request.User.PharmacyId))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            var pharmacist = _mapper.Map<Account>(request);
            return Ok(_pharmacistService.Create(pharmacist));
        }

        /// <summary>
        /// Updates an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Updated pharmacist.</response>
        /// <response code="404">Pharmacy or Pharmacist not found.</response>
        /// <response code="401">Username or email is already taken.</response>
        [Authorize(Roles = "Pharmacist, SystemAdmin")]
        [HttpPut]
        public IActionResult UpdatePharmacist(UpdatePharmacistRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.Account.Id))
                    .Or(AllDataAllowed.For(Role.SystemAdmin))
                    .Authorize();

            var pharmacist = _mapper.Map<Account>(request);
            return Ok(_pharmacistService.Update(pharmacist));
        }

        /// <summary>
        /// Deletes an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Deleted pharmacist.</response>
        /// <response code="404">Pharmacist not found.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacist(Guid id)
        {
            return Ok(_pharmacistService.Delete(id));
        }

        /// <summary>
        /// Reads the pharmacy where pharmacist works.
        /// </summary>
        /// <response code="200">Read pharmacy.</response>
        /// <response code="404">Pharmacist not found.</response>
        [HttpGet("{pharmacistId}/work-place")]
        public IActionResult GetWorkPlace(Guid pharmacistId)
        {
            return Ok(_pharmacistService.ReadWorkPlace(pharmacistId));
        }
    }
}