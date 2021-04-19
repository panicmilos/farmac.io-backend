using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("pharmacy-admins")]
    [Produces("application/json")]
    public class PharmacyAdminsController : ControllerBase
    {
        private readonly IPharmacyAdminService _pharmacyAdminService;
        private readonly IMapper _mapper;

        public PharmacyAdminsController(IPharmacyAdminService pharmacyAdminService, IMapper mapper)
        {
            _pharmacyAdminService = pharmacyAdminService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pharmacy admins from the system.
        /// </summary>
        /// <response code="200">Returns list of pharmacy admins.</response>
        [HttpGet]
        public IEnumerable<Account> GetPharmacyAdmins()
        {
            return _pharmacyAdminService.Read();
        }

        /// <summary>
        /// Returns pharmacy admin specified by id.
        /// </summary>
        /// <response code="200">Returns pharmacy admin.</response>
        /// <response code="404">Unable to return pharmacy admin because it does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult GetPharmacyAdmin(Guid id)
        {
            return Ok(_pharmacyAdminService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new pharmacy admin in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy admin.</response>
        /// <response code="400">Unable to create pharmacy admin because username or email is already taken.</response>
        /// <response code="404">Unable to create pharmacy admin because pharmacy does not exist.</response>
        [HttpPost]
        public IActionResult CreatePharmacyAdmin(CreatePharmacyAdminRequest request)
        {
            var pharmacyAdmin = _mapper.Map<Account>(request);
            _pharmacyAdminService.Create(pharmacyAdmin);

            return Ok(pharmacyAdmin);
        }

        /// <summary>
        /// Updates an existing pharmacy admin from the system.
        /// </summary>
        /// <response code="200">Returns updated pharmacist.</response>
        /// <response code="404">Unable to update pharmacy admin because pharmacy or admin does not exist.</response>
        [HttpPut]
        public IActionResult UpdatePharmacyAdmin(UpdatePharmacyAdminRequest request)
        {
            var pharmacyAdmin = _mapper.Map<Account>(request);
            var updatedPharmacyAdmin = _pharmacyAdminService.Update(pharmacyAdmin);

            return Ok(updatedPharmacyAdmin);
        }

        /// <summary>
        /// Deletes pharmacy admin from the system.
        /// </summary>
        /// <response code="200">Returns deleted pharmacy admin.</response>
        /// <response code="404">Unable to delete pharmacy admin because he does not exist.</response>
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacyAdmin(Guid id)
        {
            var deletedPharmacyAdmin = _pharmacyAdminService.Delete(id);

            return Ok(deletedPharmacyAdmin);
        }
    }
}