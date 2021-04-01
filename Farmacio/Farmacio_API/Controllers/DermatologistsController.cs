using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Farmacio_API.Contracts.Responses.Dermatologists;

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
        /// Reads all existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Read dermatologists with their work places.</response>
        [HttpGet("with-work-places")]
        public IActionResult GetDermatologistsWithWorkPlaces()
        {
            return Ok(_dermatologistService.Read().Select(da => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = da,
                WorkPlaces = _dermatologistWorkPlaceService.GetDermatologistWorkPlaces(da.User.Id)
            }));
        }

        /// <summary>
        /// Search all existing dermatologists in the system with their work places.
        /// </summary>
        /// <response code="200">Searched dermatologists with their work places.</response>
        [HttpGet("with-work-places/search")]
        public IActionResult SearchDermatologistsWithWorkPlaces(string name)
        {
            return Ok(_dermatologistService.SearchByName(name).Select(da => new DermatologistWithWorkPlacesResponse
            {
                DermatologistAccount = da,
                WorkPlaces = _dermatologistWorkPlaceService.GetDermatologistWorkPlaces(da.User.Id)
            }));
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

        /// <summary>
        /// Creates a new dermatologist in the system.
        /// </summary>
        /// <response code="200">Returns created dermatologist.</response>
        /// <response code="401">Unable to create dermatologist because username or email is already taken.</response>
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
        [HttpPut]
        public IActionResult UpdateDermatologist(UpdateDermatologistRequest request)
        {
            var dermatologist = _mapper.Map<Account>(request);
            var updatedDermatologist = _dermatologistService.Update(dermatologist);

            return Ok(updatedDermatologist);
        }

        /// <summary>
        /// Deletes dermatologist from the system.
        /// </summary>
        /// <response code="200">Returns deleted dermatologist.</response>
        /// <response code="404">Unable to delete dermatologist because he does not exist.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteDermatologist(Guid id)
        {
            var deletedDermatologist = _dermatologistService.Delete(id);

            return Ok(deletedDermatologist);
        }
    }
}