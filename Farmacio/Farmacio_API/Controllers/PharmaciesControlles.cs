using AutoMapper;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_API.Controllers
{
    [Route("pharmacies")]
    [ApiController]
    [Produces("application/json")]
    public class PharmaciesControlles : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacistService _pharmacistService;
        private readonly IMapper _mapper;

        public PharmaciesControlles(IPharmacyService pharmacyService, IPharmacistService pharmacistService, IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            _pharmacistService = pharmacistService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pharmacies from the system.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet]
        public IEnumerable<Pharmacy> ReadPharmacies()
        {
            return _pharmacyService.Read();
        }

        /// <summary>
        /// Returns all pharmacies from the system for home page with less information.
        /// </summary>
        /// <response code="200">Returns list of small pharmacy objects.</response>
        [HttpGet("home")]
        public IEnumerable<SmallPharmacyDTO> ReadForHomePage()
        {
            return _pharmacyService.ReadForHomePage();
        }

        /// <summary>
        /// Returns pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns pharmacy.</response>
        /// <response code="401">Unable to return pharmacy because it does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult ReadPharmacy(Guid id)
        {
            var pharmacy = _pharmacyService.Read(id);
            if (pharmacy == null)
            {
                throw new MissingEntityException();
            }

            return Ok(pharmacy);
        }
        
        /// <summary>
        /// Reads all existing pharmacists employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read pharmacists from the pharmacy.</response>
        [HttpGet("{pharmacyId}/pharmacists")]
        public IActionResult GetPharmacists(Guid pharmacyId)
        {
            return Ok(_pharmacistService.ReadForPharmacy(pharmacyId));
        }
        
        /// <summary>
        /// Search all existing pharmacists in the system.
        /// </summary>
        /// <response code="200">Searched pharmacists.</response>
        [HttpGet("{pharmacyId}/pharmacists/search")]
        public IActionResult SearchPharmacists(Guid pharmacyId, string name)
        {
            return Ok(_pharmacistService.SearchByNameForPharmacy(pharmacyId, name));
        }
        
        /// <summary>
        /// Reads an existing pharmacist employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read pharmacist from the pharmacy.</response>
        [HttpGet("{pharmacyId}/pharmacists/{pharmacistId}")]
        public IActionResult GetPharmacist(Guid pharmacyId, Guid pharmacistId)
        {
            return Ok(_pharmacistService.ReadForPharmacy(pharmacyId, pharmacistId));
        }

        /// <summary>
        /// Creates a new pharmacy in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy.</response>
        [HttpPost]
        public IActionResult CreatePharmacy(CreatePharmacyRequest request)
        {
            var pharmacy = _mapper.Map<Pharmacy>(request);
            _pharmacyService.Create(pharmacy);

            return Ok(pharmacy);
        }

        /// <summary>
        /// Deleted pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns deleted pharmacy.</response>
        /// <response code="401">Unable to delete pharmacy because it does not exist in the system.</response>
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacy(Guid id)
        {
            var deletedPharmacy = _pharmacyService.Delete(id);

            return Ok(deletedPharmacy);
        }
    }
}