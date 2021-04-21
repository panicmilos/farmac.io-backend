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
        private readonly IPharmacyOrderService _pharmacyOrderService;
        private readonly IMapper _mapper;

        public PharmacyOrdersController( IPharmacyOrderService pharmacyOrderService
            , IMapper mapper)
        {
            _pharmacyOrderService = pharmacyOrderService;
            _mapper = mapper;
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
        public IActionResult UpdatePharmacyOrder(Guid pharmacyId, CreatePharmacyOrderRequest pharmacyOrderRequest)
        {
            var pharmacyOrder = _mapper.Map<PharmacyOrder>(pharmacyOrderRequest);
            pharmacyOrder.PharmacyId = pharmacyId;

            return Ok(_pharmacyOrderService.Update(pharmacyOrder));
        }
    }
}
