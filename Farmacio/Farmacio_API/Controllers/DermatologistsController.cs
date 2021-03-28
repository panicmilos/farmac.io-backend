using System;
using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("dermatologists")]
    [Produces("application/json")]
    public class DermatologistsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDermatologistService _dermatologistService;
        
        public DermatologistsController(IDermatologistService dermatologistService, IMapper mapper)
        {
            _dermatologistService = dermatologistService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Reads all existing dermatologists in the system.
        /// </summary>
        /// <response code="200">Read dermatologists.</response>
        [HttpGet]
        public IActionResult GetDermatologists()
        {
            return Ok(_dermatologistService.Read());
        }
        
        /// <summary>
        /// Search all existing dermatologists in the system.
        /// </summary>
        /// <response code="200">Searched dermatologists.</response>
        [HttpGet("search")]
        public IActionResult SearchDermatologists(string name)
        {
            return Ok(_dermatologistService.SearchByName(name));
        }
        
        /// <summary>
        /// Reads an existing dermatologist in the system.
        /// </summary>
        /// <response code="200">Read dermatologist.</response>
        [HttpGet("{id}")]
        public IActionResult GetDermatologist(Guid id)
        {
            return Ok(_dermatologistService.Read(id));
        }

        /// <summary>
        /// Reads all dermatologist's patients.
        /// </summary>
        /// <response code="200">Read patients.</response>
        [HttpGet("{id}/patients")]
        public IActionResult GetPatients(Guid id)
        {
            return Ok(_dermatologistService.GetPatients(id));
        }
    }
}