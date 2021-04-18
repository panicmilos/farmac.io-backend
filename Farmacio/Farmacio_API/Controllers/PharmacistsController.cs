using System;
using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public IActionResult GetPharmacists()
        {
            return Ok(_pharmacistService.Read());
        }

        /// <summary>
        /// Search all existing pharmacists in the system.
        /// </summary>
        /// <response code="200">Searched pharmacists.</response>
        [HttpGet("search")]
        public IActionResult SearchPharmacists(string name)
        {
            return Ok(_pharmacistService.SearchByName(name));
        }

        /// <summary>
        /// Reads an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Read pharmacist.</response>
        [HttpGet("{id}")]
        public IActionResult GetPharmacist(Guid id)
        {
            return Ok(_pharmacistService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new pharmacist in the system.
        /// </summary>
        /// <response code="200">Created pharmacist.</response>
        /// <response code="404">Pharmacy not found.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPost]
        public IActionResult CreatePharmacist(CreatePharmacistRequest request)
        {
            var pharmacist = _mapper.Map<Account>(request);
            return Ok(_pharmacistService.Create(pharmacist));
        }

        /// <summary>
        /// Updates an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Updated pharmacist.</response>
        /// <response code="404">Pharmacy or Pharmacist not found.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPut]
        public IActionResult UpdatePharmacist(UpdatePharmacistRequest request)
        {
            var pharmacist = _mapper.Map<Account>(request);
            return Ok(_pharmacistService.Update(pharmacist));
        }

        /// <summary>
        /// Deletes an existing pharmacist in the system.
        /// </summary>
        /// <response code="200">Deleted pharmacist.</response>
        /// <response code="404">Pharmacist not found.</response>
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacist(Guid id)
        {
            return Ok(_pharmacistService.Delete(id));
        }
    }
}