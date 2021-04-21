using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_Models.Domain;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("pharmacy-orders")]
    [Produces("application/json")]
    public class PharmacyOrdersController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacyAdminService _pharmacyAdminService;
        private readonly IMedicineService _medicineService;
        private readonly IPharmacyOrderService _pharmacyOrderService;
        private readonly IMapper _mapper;

        public PharmacyOrdersController(IPharmacyService pharmacyService, IPharmacyAdminService pharmacyAdminService
            , IMedicineService medicineService, IPharmacyOrderService pharmacyOrderService
            , IMapper mapper)
        {
            _pharmacyOrderService = pharmacyOrderService;
            _pharmacyService = pharmacyService;
            _pharmacyAdminService = pharmacyAdminService;
            _medicineService = medicineService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Create a pharmacy order.
        /// </summary>
        /// <response code="200">Created pharmacy order.</response>
        /// <response code="404">Medicine, Pharmacy admin or Pharmacy not found.</response>
        [HttpPost("{pharmacyId}")]
        public IActionResult AddMedicineToPharmacy(Guid pharmacyId, CreatePharmacyOrderRequest pharmacyOrderRequest)
        {
            var pharmacyOrder = _mapper.Map<PharmacyOrder>(pharmacyOrderRequest);
            pharmacyOrder.PharmacyId = pharmacyId;
            
            _pharmacyService.TryToRead(pharmacyOrder.PharmacyId);
            _pharmacyAdminService.TryToRead(pharmacyOrder.PharmacyAdminId);
            pharmacyOrder.OrderedMedicines.ForEach(orderedMedicine =>
                _medicineService.TryToRead(orderedMedicine.MedicineId));

            return Ok(_pharmacyOrderService.Create(pharmacyOrder));
        }
    }
}
