using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_Models.Domain;
using GlobalExceptionHandler.Exceptions;

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
