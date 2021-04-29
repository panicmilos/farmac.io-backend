using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Contracts.Requests.Promotions;
using Farmacio_Models.Domain;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PomotionsController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPromotionService _promotionService;
        private readonly IMapper _mapper;

        public PomotionsController(IPharmacyService pharmacyService, IPromotionService promotionService
            , IMapper mapper)
        {
            _promotionService = promotionService;
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Read existing promotions for pharmacy.
        /// </summary>
        /// <response code="200">Read promotions.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/promotions")]
        public IActionResult ReadPharmacyPromotions(Guid pharmacyId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_promotionService.ReadFor(pharmacyId));
        }
        
        /// <summary>
        /// Read active promotions for pharmacy.
        /// </summary>
        /// <response code="200">Read active promotions.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/promotions/active")]
        public IActionResult ReadActivePharmacyPromotions(Guid pharmacyId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_promotionService.ReadActiveFor(pharmacyId));
        }
        
        /// <summary>
        /// Read an existing promotion by id.
        /// </summary>
        /// <response code="200">Read promotion.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/promotions/{promotionId}")]
        public IActionResult ReadPharmacyPromotion(Guid pharmacyId, Guid promotionId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            var promotion = _promotionService.TryToRead(promotionId);
            if (promotion.PharmacyId != pharmacyId)
               throw new UnauthorizedAccessException("Promotion does not belong to the given pharmacy."); 
            return Ok(promotion);
        }

        /// <summary>
        /// Create a promotion.
        /// </summary>
        /// <response code="200">Created promotion.</response>
        /// <response code="400">Invalid time interval or discount.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpPost("/pharmacy/{pharmacyId}/promotions")]
        public IActionResult CreatePharmacyPromotion(Guid pharmacyId, CreatePromotionRequest promotionRequest)
        {
            var promotion = _mapper.Map<Promotion>(promotionRequest);
            promotion.PharmacyId = pharmacyId;

            return Ok(_promotionService.Create(promotion));
        }
        
        /// <summary>
        /// Update existing promotion.
        /// </summary>
        /// <response code="200">Updated promotion.</response>
        /// <response code="400">Invalid time interval or discount.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpPut("/pharmacy/{pharmacyId}/promotions")]
        public IActionResult UpdatePharmacyPromotion(Guid pharmacyId, UpdatePromotionRequest promotionRequest)
        {
            var promotion = _mapper.Map<Promotion>(promotionRequest);
            promotion.PharmacyId = pharmacyId;

            return Ok(_promotionService.Update(promotion));
        }
        
        /// <summary>
        /// Delete existing promotion.
        /// </summary>
        /// <response code="200">Deleted promotion.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpDelete("/pharmacy/{pharmacyId}/promotions/{promotionId}")]
        public IActionResult DeletePharmacyPromotion(Guid pharmacyId, Guid promotionId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_promotionService.Delete(promotionId));
        }
    }
}
