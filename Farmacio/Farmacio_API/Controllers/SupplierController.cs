using AutoMapper;
using Farmacio_API.Contracts.Requests.SupplierOffers;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.SupplierMedicines;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("suppliers")]
    [Produces("application/json")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierStockService _supplierStockService;
        private readonly ISupplierOfferService _supplierOfferService;
        private readonly IMapper _mapper;

        public SupplierController(
            ISupplierService supplierService,
            ISupplierStockService supplierStockService,
            ISupplierOfferService supplierOfferService,
            IMapper mapper)
        {
            _supplierService = supplierService;
            _supplierStockService = supplierStockService;
            _supplierOfferService = supplierOfferService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all suppliers from the system.
        /// </summary>
        /// <response code="200">Returns list of suppliers.</response>
        [HttpGet]
        public IEnumerable<Account> GetSuppliers()
        {
            return _supplierService.Read();
        }

        /// <summary>
        /// Returns supplier specified by id.
        /// </summary>
        /// <response code="200">Returns supplier.</response>
        /// <response code="404">Unable to return supplier because he does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult GetSupplier(Guid id)
        {
            return Ok(_supplierService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new supplier in the system.
        /// </summary>
        /// <response code="200">Returns created supplier.</response>
        /// <response code="400">Unable to create supplier because username or email is already taken.</response>
        [HttpPost]
        public IActionResult CreateSupplier(CreateSupplierRequest request)
        {
            var supplier = _mapper.Map<Account>(request);
            _supplierService.Create(supplier);

            return Ok(supplier);
        }

        /// <summary>
        /// Updates an existing supplier from the system.
        /// </summary>
        /// <response code="200">Returns updated supplier.</response>
        /// <response code="404">Unable to update supplier because he does not exist.</response>
        [HttpPut]
        public IActionResult UpdateSupplier(UpdateSupplierRequest request)
        {
            var supplier = _mapper.Map<Account>(request);
            var updatedSupplier = _supplierService.Update(supplier);

            return Ok(updatedSupplier);
        }

        /// <summary>
        /// Deletes supplier from the system.
        /// </summary>
        /// <response code="200">Returns deleted supplier.</response>
        /// <response code="404">Unable to delete supplier because he does not exist.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(Guid id)
        {
            var deletedSupplier = _supplierService.Delete(id);

            return Ok(deletedSupplier);
        }

        /// <summary>
        /// Returns all supplier's medicines from the stock.
        /// </summary>
        /// <response code="200">Returns list of supplier's medicines.</response>
        [HttpGet("{supplierId}/medicines-in-stock")]
        public IActionResult GetMedicinesInStock(Guid supplierId)
        {
            return Ok(_supplierStockService.ReadFor(supplierId));
        }

        /// <summary>
        /// Adds a medicine to the supplier's stock.
        /// </summary>
        /// <response code="200">Returns created supplier's medicine.</response>
        /// <response code="400">Unable to add medicine to supplier's stock because it is already there.</response>
        /// <response code="404">Given supplier or medicine doesn't exist.</response>
        [HttpPost("{supplierId}/medicines-in-stock")]
        public IActionResult AddMedicineToSupplierStock(CreateSupplierMedicineRequest request)
        {
            var medicine = _mapper.Map<SupplierMedicine>(request);
            _supplierStockService.Create(medicine);

            return Ok(medicine);
        }

        /// <summary>
        /// Updates quantity of medicine from the supplier's stock.
        /// </summary>
        /// <response code="200">Returns updated supplier's medicine.</response>
        /// <response code="404">Given supplier's medicine doesn't exist.</response>
        [HttpPut("{supplierId}/medicines-in-stock")]
        public IActionResult UpdateMedicineFromSupplierStock(UpdateSupplierMedicineRequest request)
        {
            var medicine = _mapper.Map<SupplierMedicine>(request);
            var updatedMedicine = _supplierStockService.Update(medicine);

            return Ok(updatedMedicine);
        }

        /// <summary>
        /// Deletes supplier's medicine from the stock.
        /// </summary>
        /// <response code="200">Returns deleted supplier's medicine.</response>
        /// <response code="404">Given supplier's medicine doesn't exist.</response>
        [HttpDelete("{supplierId}/medicines-in-stock/{id}")]
        public IActionResult DeleteMedicineFromSupplierStock(Guid id)
        {
            var deletedMedicine = _supplierStockService.Delete(id);

            return Ok(deletedMedicine);
        }  

        /// <summary>
        /// Returns all supplier's offers from the system.
        /// </summary>
        /// <response code="200">Returns list of supplier's offers.</response>
        [HttpGet("{supplierId}/offers")]
        public IActionResult GetSuppliersOffers(Guid supplierId)
        {
            return Ok(_supplierOfferService.ReadFor(supplierId));
        }
        
        /// <summary>
        /// Returns all supplier's offers for a pharmacy order.
        /// </summary>
        /// <response code="200">Returns list of supplier's offers for a pharmacy order.</response>
        [HttpGet("offers/pharmacy-order/{pharmacyOrderId}")]
        public IActionResult GetSuppliersOffersForPharmacyOrder(Guid pharmacyOrderId)
        {
            return Ok(_supplierOfferService.ReadForPharmacyOrder(pharmacyOrderId));
        }
        
         /// <summary>
        /// Creates a new supplier's offer for pharmacy order in the system.
        /// </summary>
        /// <response code="200">Returns created supplier's offer.</response>
        /// <response code="400">
        /// Unable to create supplier's offer because supplier already gave offer
        /// for given order or delivery date is after deadline or
        /// order is already processed or supplier don't have enough medicines.
        /// </response>
        /// <response code="404">Given supplier or pharmacy order doesn't exist.</response>
        [HttpPost("{supplierId}/offers")]
        public IActionResult CreateSupplierOffer(CreateSupplierOfferRequest request)
        {
            var offer = _mapper.Map<SupplierOffer>(request);
            _supplierOfferService.Create(offer);

            return Ok(offer);
        }

        /// <summary>
        /// Updates an existing supplier's offer for pharmacy order from the system.
        /// </summary>
        /// <response code="200">Returns updated supplier's offer.</response>
        /// <response code="400">
        /// Unable to update supplier's offer because delivery date is after deadline or
        /// order is already processed.
        /// </response>
        /// <response code="404">Given offer doesn't exist.</response>
        [HttpPut("{supplierId}/offers")]
        public IActionResult UpdateSupplierOffer(UpdateSupplierOfferRequest request)
        {
            var offer = _mapper.Map<SupplierOffer>(request);
            var updatedOffer = _supplierOfferService.Update(offer);

            return Ok(updatedOffer);
        }
    }
}