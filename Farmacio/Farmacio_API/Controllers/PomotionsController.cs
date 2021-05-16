using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Contracts.Requests.Promotions;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Authorization;

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
            return Ok(_promotionService.ReadFor(pharmacyId));
        }
        
        /// <summary>
        /// Read existing promotions page for pharmacy.
        /// </summary>
        /// <response code="200">Read promotions page.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/promotions/page")]
        public IActionResult ReadPharmacyPromotionsPage(Guid pharmacyId, int number, int size)
        {
            return Ok(_promotionService.ReadPageFor(pharmacyId, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }
        
        /// <summary>
        /// Read active promotions for pharmacy.
        /// </summary>
        /// <response code="200">Read active promotions.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/promotions/active")]
        public IActionResult ReadActivePharmacyPromotions(Guid pharmacyId)
        {
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
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("/pharmacy/{pharmacyId}/promotions")]
        public IActionResult CreatePharmacyPromotion(Guid pharmacyId, CreatePromotionRequest promotionRequest)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
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
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPut("/pharmacy/{pharmacyId}/promotions")]
        public IActionResult UpdatePharmacyPromotion(Guid pharmacyId, UpdatePromotionRequest promotionRequest)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            var promotion = _mapper.Map<Promotion>(promotionRequest);
            promotion.PharmacyId = pharmacyId;

            return Ok(_promotionService.Update(promotion));
        }
        
        /// <summary>
        /// Delete existing promotion.
        /// </summary>
        /// <response code="200">Deleted promotion.</response>
        /// <response code="404">Pharmacy not found.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpDelete("/pharmacy/{pharmacyId}/promotions/{promotionId}")]
        public IActionResult DeletePharmacyPromotion(Guid pharmacyId, Guid promotionId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_promotionService.Delete(promotionId));
        }
    }
}
