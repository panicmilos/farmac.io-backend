using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

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
        [HttpGet("pharmacies/{pharmacyId}/not-in-stocks")]
        public IActionResult ReadNotInStockRecordsFor(Guid pharmacyId)
        {
            return Ok(_notInStockService.ReadFor(pharmacyId));
        }

        /// <summary>
        /// Marks not in stock record as seen.
        /// </summary>
        /// <response code="200">Marked seen not in stock record.</response>
        /// <response code="404">Not in stock record not found.</response>
        [HttpPut("not-in-stocks/{notInStockId}/seen")]
        public IActionResult MarkNotInStockRecordAsSeen(Guid notInStockId)
        {
            return Ok(_notInStockService.MarkSeen(notInStockId));
        }
    }
}