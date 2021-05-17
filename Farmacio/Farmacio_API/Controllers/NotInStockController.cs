using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Farmacio_Models.DTO;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class NotInStockController : ControllerBase
    {
        private readonly INotInStockService _notInStockService;

        public NotInStockController(INotInStockService notInStockService)
        {
            _notInStockService = notInStockService;
        }

        /// <summary>
        /// Reads not in stock records in a pharmacy.
        /// </summary>
        /// <response code="200">Read not in stock records.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("pharmacies/{pharmacyId}/not-in-stocks")]
        public IActionResult ReadNotInStockRecordsFor(Guid pharmacyId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_notInStockService.ReadFor(pharmacyId));
        }

        /// <summary>
        /// Filters not in stock records page by seen status in a pharmacy.
        /// </summary>
        /// <response code="200">Filtered not in stock records page.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("pharmacies/{pharmacyId}/not-in-stocks/page")]
        public IActionResult FilterNotInStockRecordsPageFor(Guid pharmacyId, bool? isSeen, int number, int size)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_notInStockService.ReadPageFor(pharmacyId, isSeen, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }
        
        /// <summary>
        /// Marks not in stock record as seen.
        /// </summary>
        /// <response code="200">Marked seen not in stock record.</response>
        /// <response code="404">Not in stock record not found.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPut("not-in-stocks/{notInStockId}/seen")]
        public IActionResult MarkNotInStockRecordAsSeen(Guid notInStockId)
        {
            var notInStock = _notInStockService.Read(notInStockId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(notInStock.PharmacyId))
                .Authorize();
            return Ok(_notInStockService.MarkSeen(notInStockId));
        }
    }
}