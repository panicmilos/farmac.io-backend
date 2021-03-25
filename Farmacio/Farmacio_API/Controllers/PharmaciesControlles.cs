using AutoMapper;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [Route("pharmacies")]
    [ApiController]
    [Produces("application/json")]
    public class PharmaciesControlles : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IMapper _mapper;

        public PharmaciesControlles(IPharmacyService pharmacyService, IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pharmacies from the system..
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet]
        public IEnumerable<Pharmacy> ReadPharmacies()
        {
            return _pharmacyService.Read();
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