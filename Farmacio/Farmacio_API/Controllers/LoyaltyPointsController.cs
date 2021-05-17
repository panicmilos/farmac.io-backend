using AutoMapper;
using Farmacio_API.Contracts.Requests.LoyaltyPoints;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [Route("loyalty-points")]
    [ApiController]
    [Produces("application/json")]
    public class LoyaltyPointsController : ControllerBase
    {
        private readonly ILoyaltyPointsService _loyaltyPointsService;
        private readonly IMapper _mapper;

        public LoyaltyPointsController(
            ILoyaltyPointsService loyaltyPointsService,
            IMapper mapper)
        {
            _loyaltyPointsService = loyaltyPointsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns loyalty points from the system.
        /// </summary>
        /// <response code="200">Returns loyalty points.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IActionResult GetLoyaltyPoints()
        {
            return Ok(_loyaltyPointsService.ReadOrCreate());
        }

        /// <summary>
        /// Updates an existing loyalty points from the system.
        /// </summary>
        /// <response code="200">Returns updated loyalty points.</response>
        /// <response code="404">Given medicine doesn't exist.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpPut]
        public IActionResult UpdateLoyaltyPoints(UpdateLoyaltyPointsRequest request)
        {
            var loyaltyPoints = _mapper.Map<LoyaltyPoints>(request);
            var updatedLoyaltyPoints = _loyaltyPointsService.Update(loyaltyPoints);

            return Ok(updatedLoyaltyPoints);
        }
    }
}