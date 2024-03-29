﻿using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IEnumerable<Account> GetPharmacyAdmins()
        {
            return _pharmacyAdminService.Read();
        }

        /// <summary>
        /// Returns page of pharmacy admins from the system.
        /// </summary>
        /// <response code="200">Returns page of pharmacy admins.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("page")]
        public IEnumerable<Account> GetPharmacyAdminsPage([FromQuery] PageDTO page)
        {
            return _pharmacyAdminService.ReadPageOf(Role.PharmacyAdmin, page);
        }

        /// <summary>
        /// Returns pharmacy admin specified by id.
        /// </summary>
        /// <response code="200">Returns pharmacy admin.</response>
        /// <response code="404">Unable to return pharmacy admin because it does not exist in the system.</response>
        [Authorize(Roles = "SystemAdmin, PharmacyAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetPharmacyAdmin(Guid id)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(id))
                    .Or(AllDataAllowed.For(Role.SystemAdmin))
                    .Authorize();

            return Ok(_pharmacyAdminService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new pharmacy admin in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy admin.</response>
        /// <response code="400">Unable to create pharmacy admin because username or email is already taken.</response>
        /// <response code="404">Unable to create pharmacy admin because pharmacy does not exist.</response>
        [Authorize(Roles = "SystemAdmin")]
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
        [Authorize(Roles = "SystemAdmin, PharmacyAdmin")]
        [HttpPut]
        public IActionResult UpdatePharmacyAdmin(UpdatePharmacyAdminRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(request.Account.Id))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            var pharmacyAdmin = _mapper.Map<Account>(request);
            var updatedPharmacyAdmin = _pharmacyAdminService.Update(pharmacyAdmin);

            return Ok(updatedPharmacyAdmin);
        }

        /// <summary>
        /// Deletes pharmacy admin from the system.
        /// </summary>
        /// <response code="200">Returns deleted pharmacy admin.</response>
        /// <response code="404">Unable to delete pharmacy admin because he does not exist or is the last admin of the pharmacy.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacyAdmin(Guid id)
        {
            var deletedPharmacyAdmin = _pharmacyAdminService.Delete(id);

            return Ok(deletedPharmacyAdmin);
        }
    }
}