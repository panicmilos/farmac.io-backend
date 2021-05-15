using AutoMapper;
using Farmacio_API.Contracts.Requests.LoyaltyPrograms;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [Route("loyalty-programs")]
    [ApiController]
    [Produces("application/json")]
    public class LoyaltyProgramsController : ControllerBase
    {
        private readonly ILoyaltyProgramService _loyaltyProgramService;
        private readonly IMapper _mapper;

        public LoyaltyProgramsController(ILoyaltyProgramService loyaltyProgramService, IMapper mapper)
        {
            _loyaltyProgramService = loyaltyProgramService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all loyalty programs from the system.
        /// </summary>
        /// <response code="200">List of loyalty programs.</response>
        [HttpGet]
        public IActionResult ReadLoyaltyPrograms()
        {
            return Ok(_loyaltyProgramService.Read());
        }

        /// <summary>
        /// Returns page of loyalty programs from the system.
        /// </summary>
        /// <response code="200">Page of loyalty programs.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("page")]
        public IActionResult ReadLoyaltyProgramsPage([FromQuery] PageDTO page)
        {
            return Ok(_loyaltyProgramService.ReadPage(page));
        }

        /// <summary>
        /// Returns discount percentage for a given patient.
        /// </summary>
        /// <response code="200">Discount.</response>
        /// <response code="404">Given patient is not found.</response>
        [HttpGet("discount-for/{patientId}")]
        public IActionResult ReadDiscountFor(Guid patientId)
        {
            return Ok(_loyaltyProgramService.ReadDiscountFor(patientId));
        }

        /// <summary>
        /// Creates a new loyalty program in the system.
        /// </summary>
        /// <response code="200">Created loyalty program.</response>
        /// <response code="400">Loyalty program with given minimum points already exists.</response>
        [HttpPost]
        public IActionResult CreateLoyaltyProgram(CreateLoyaltyProgramRequest request)
        {
            var loyaltyProgram = _mapper.Map<LoyaltyProgram>(request);
            var createdLoyaltyProgram = _loyaltyProgramService.Create(loyaltyProgram);

            return Ok(createdLoyaltyProgram);
        }

        /// <summary>
        /// Updates an existing loyalty program from the system.
        /// </summary>
        /// <response code="200">Updated loyalty program.</response>
        /// <response code="400">Loyalty program with given minimum points already exists.</response>
        /// <response code="404">Given loyalty program doesn't exist.</response>
        [HttpPut]
        public IActionResult UpdateLoyaltyProgram(UpdateLoyaltyProgramRequest request)
        {
            var loyaltyProgram = _mapper.Map<LoyaltyProgram>(request);
            var updatedLoyaltyProgram = _loyaltyProgramService.Update(loyaltyProgram);

            return Ok(updatedLoyaltyProgram);
        }

        /// <summary>
        /// Deletes an existing loyalty program from the system.
        /// </summary>
        /// <response code="200">Deleted loyalty program.</response>
        /// <response code="404">Given loyalty program doesn't exist.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteLoyaltyProgram(Guid id)
        {
            var deletedLoyaltyProgram = _loyaltyProgramService.Delete(id);

            return Ok(deletedLoyaltyProgram);
        }
    }
}