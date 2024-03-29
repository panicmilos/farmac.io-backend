﻿using AutoMapper;
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
    [Route("system-admins")]
    [Produces("application/json")]
    public class SystemAdminsContollers : ControllerBase
    {
        private readonly ISystemAdminService _systemAdminService;
        private readonly IMapper _mapper;

        public SystemAdminsContollers(ISystemAdminService systemAdminService, IMapper mapper)
        {
            _systemAdminService = systemAdminService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all system admins from the system.
        /// </summary>
        /// <response code="200">Returns list of system admins.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IEnumerable<Account> GetSystemAdmins()
        {
            return _systemAdminService.Read();
        }

        /// <summary>
        /// Returns page of system admins from the system.
        /// </summary>
        /// <response code="200">Returns page of system admins.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("page")]
        public IEnumerable<Account> GetSystemAdminsPage([FromQuery] PageDTO page)
        {
            return _systemAdminService.ReadPageOf(Role.SystemAdmin, page);
        }

        /// <summary>
        /// Returns system admin specified by id.
        /// </summary>
        /// <response code="200">Returns system admin.</response>
        /// <response code="404">Unable to return system admin because it does not exist in the system.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetSystemAdmin(Guid id)
        {
            return Ok(_systemAdminService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new system admin in the system.
        /// </summary>
        /// <response code="200">Returns system pharmacy.</response>
        /// <response code="401">Unable to create system admin because username or email is already taken.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpPost]
        public IActionResult CreateSystemAdmin(CreateSystemAdminRequest request)
        {
            var systemAdmin = _mapper.Map<Account>(request);
            _systemAdminService.Create(systemAdmin);

            return Ok(systemAdmin);
        }

        /// <summary>
        /// Updates an existing system admin from the system.
        /// </summary>
        /// <response code="200">Returns system pharmacist.</response>
        /// <response code="404">Unable to update system admin because he does not exist.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpPut]
        public IActionResult UpdateSystemAdmin(UpdateSystemAdminRequest request)
        {
            var systemAdmin = _mapper.Map<Account>(request);
            var updatedSystemAdmin = _systemAdminService.Update(systemAdmin);

            return Ok(updatedSystemAdmin);
        }

        /// <summary>
        /// Deletes system admin from the system.
        /// </summary>
        /// <response code="200">Returns deleted system admin.</response>
        /// <response code="404">Unable to delete system admin because he does not exist or is the last system admin.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteSystemAdmin(Guid id)
        {
            var deletedSystemAdmin = _systemAdminService.Delete(id);

            return Ok(deletedSystemAdmin);
        }
    }
}