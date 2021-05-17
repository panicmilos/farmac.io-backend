using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_Models.DTO;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PharmacyOrdersController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacyOrderService _pharmacyOrderService;
        private readonly IMapper _mapper;

        public PharmacyOrdersController(IPharmacyService pharmacyService, IPharmacyOrderService pharmacyOrderService
            , IMapper mapper)
        {
            _pharmacyOrderService = pharmacyOrderService;
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Read an existing pharmacy order by id.
        /// </summary>
        /// <response code="200">Read pharmacy order.</response>
        /// <response code="404">Pharmacy or PharmacyOrder not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/pharmacy-orders/{pharmacyOrderId}")]
        public IActionResult ReadPharmacyOrder(Guid pharmacyId, Guid pharmacyOrderId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_pharmacyOrderService.TryToRead(pharmacyOrderId));
        }

        /// <summary>
        /// Filter existing pharmacy orders by processed status.
        /// </summary>
        /// <response code="200">Filtered pharmacy orders.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/pharmacy-orders/filter")]
        public IActionResult FilterPharmacyOrders(Guid pharmacyId, [FromQuery] bool? isProcessed)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_pharmacyOrderService.ReadFor(pharmacyId, isProcessed));
        }

        /// <summary>
        /// Paginated filter existing pharmacy orders by processed status.
        /// </summary>
        /// <response code="200">Filtered pharmacy orders page.</response>
        /// <response code="404">Pharmacy not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/pharmacy-orders/filter/page")]
        public IActionResult FilterPharmacyOrders(Guid pharmacyId, bool? isProcessed, int number, int size)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_pharmacyOrderService.ReadPageFor(pharmacyId, isProcessed, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }
        
        /// <summary>
        /// Create a pharmacy order.
        /// </summary>
        /// <response code="200">Created pharmacy order.</response>
        /// <response code="404">Medicine, Pharmacy admin or Pharmacy not found.</response>
        [HttpPost("/pharmacy/{pharmacyId}/pharmacy-orders")]
        public IActionResult CreatePharmacyOrder(Guid pharmacyId, CreatePharmacyOrderRequest pharmacyOrderRequest)
        {
            var pharmacyOrder = _mapper.Map<PharmacyOrder>(pharmacyOrderRequest);
            pharmacyOrder.PharmacyId = pharmacyId;

            return Ok(_pharmacyOrderService.Create(pharmacyOrder));
        }

        /// <summary>
        /// Update existing pharmacy order.
        /// </summary>
        /// <response code="200">Updated pharmacy order.</response>
        /// <response code="404">Medicine, Pharmacy admin or Pharmacy not found.</response>
        /// <response code="400">Supplier offer has been created for the provided pharmacy order.</response>
        [HttpPut("/pharmacy/{pharmacyId}/pharmacy-orders")]
        public IActionResult UpdatePharmacyOrder(Guid pharmacyId, UpdatePharmacyOrderRequest pharmacyOrderRequest)
        {
            var pharmacyOrder = _mapper.Map<PharmacyOrder>(pharmacyOrderRequest);
            pharmacyOrder.PharmacyId = pharmacyId;

            return Ok(_pharmacyOrderService.Update(pharmacyOrder));
        }
    }
}