using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.SupplierMedicines;
using Farmacio_API.Contracts.Requests.SupplierOffers;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Farmacio_Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Farmacio_API.Authorization;

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
        private readonly IPharmacyOrderService _pharmacyOrderService;
        private readonly ICrudService<SupplierMedicine> _supplierMedicineService;
        private readonly IMapper _mapper;

        public SupplierController(
            ISupplierService supplierService,
            ISupplierStockService supplierStockService,
            ISupplierOfferService supplierOfferService,
            IPharmacyOrderService pharmacyOrderService,
            ICrudService<SupplierMedicine> supplierMedicineService,
            IMapper mapper)
        {
            _supplierService = supplierService;
            _supplierStockService = supplierStockService;
            _supplierOfferService = supplierOfferService;
            _pharmacyOrderService = pharmacyOrderService;
            _supplierMedicineService = supplierMedicineService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all suppliers from the system.
        /// </summary>
        /// <response code="200">Returns list of suppliers.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IEnumerable<Account> GetSuppliers()
        {
            return _supplierService.Read();
        }

        /// <summary>
        /// Returns page of suppliers from the system.
        /// </summary>
        /// <response code="200">Returns page of suppliers.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("page")]
        public IEnumerable<Account> GetSuppliersPage([FromQuery] PageDTO page)
        {
            return _supplierService.ReadPageOf(Role.Supplier, page);
        }

        /// <summary>
        /// Returns supplier specified by id.
        /// </summary>
        /// <response code="200">Returns supplier.</response>
        /// <response code="404">Unable to return supplier because he does not exist in the system.</response>
        [Authorize(Roles = "Supplier, SystemAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetSupplier(Guid id)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(id))
                    .Or(AllDataAllowed.For(Role.SystemAdmin))
                    .Authorize();

            return Ok(_supplierService.TryToRead(id));
        }

        /// <summary>
        /// Creates a new supplier in the system.
        /// </summary>
        /// <response code="200">Returns created supplier.</response>
        /// <response code="400">Unable to create supplier because username or email is already taken.</response>
        [Authorize(Roles = "SystemAdmin")]
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
        [Authorize(Roles = "Supplier, SystemAdmin")]
        [HttpPut]
        public IActionResult UpdateSupplier(UpdateSupplierRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.Account.Id))
                    .Or(AllDataAllowed.For(Role.SystemAdmin))
                    .Authorize();

            var supplier = _mapper.Map<Account>(request);
            var updatedSupplier = _supplierService.Update(supplier);

            return Ok(updatedSupplier);
        }

        /// <summary>
        /// Deletes supplier from the system.
        /// </summary>
        /// <response code="200">Returns deleted supplier.</response>
        /// <response code="404">Unable to delete supplier because he does not exist.</response>
        [Authorize(Roles = "SystemAdmin")]
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
        [Authorize(Roles = "Supplier")]
        [HttpGet("{supplierId}/medicines-in-stock")]
        public IActionResult GetMedicinesInStock(Guid supplierId)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierId))
                    .Authorize();

            return Ok(_supplierStockService.ReadFor(supplierId));
        }

        /// <summary>
        /// Returns page of supplier's medicines from the stock.
        /// </summary>
        /// <response code="200">Returns page of supplier's medicines.</response>
        [Authorize(Roles = "Supplier")]
        [HttpGet("{supplierId}/medicines-in-stock/page")]
        public IActionResult GetMedicinesInStockPage(Guid supplierId, [FromQuery] PageDTO page)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierId))
                    .Authorize();

            return Ok(_supplierStockService.ReadPageOfMedicinesFor(supplierId, page));
        }

        /// <summary>
        /// Adds a medicine to the supplier's stock.
        /// </summary>
        /// <response code="200">Returns created supplier's medicine.</response>
        /// <response code="400">Unable to add medicine to supplier's stock because it is already there.</response>
        /// <response code="404">Given supplier or medicine doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpPost("{supplierId}/medicines-in-stock")]
        public IActionResult AddMedicineToSupplierStock(CreateSupplierMedicineRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.SupplierId))
                    .Authorize();

            var medicine = _mapper.Map<SupplierMedicine>(request);
            _supplierStockService.Create(medicine);

            return Ok(medicine);
        }

        /// <summary>
        /// Updates quantity of medicine from the supplier's stock.
        /// </summary>
        /// <response code="200">Returns updated supplier's medicine.</response>
        /// <response code="404">Given supplier's medicine doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpPut("{supplierId}/medicines-in-stock")]
        public IActionResult UpdateMedicineFromSupplierStock(UpdateSupplierMedicineRequest request)
        {
            var supplierMedicine = _supplierMedicineService.TryToRead(request.Id);
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierMedicine.SupplierId))
                    .Authorize();

            var medicine = _mapper.Map<SupplierMedicine>(request);
            var updatedMedicine = _supplierStockService.Update(medicine);

            return Ok(updatedMedicine);
        }

        /// <summary>
        /// Deletes supplier's medicine from the stock.
        /// </summary>
        /// <response code="200">Returns deleted supplier's medicine.</response>
        /// <response code="404">Given supplier's medicine doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpDelete("{supplierId}/medicines-in-stock/{id}")]
        public IActionResult DeleteMedicineFromSupplierStock(Guid id)
        {
            var supplierMedicine = _supplierMedicineService.TryToRead(id);
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierMedicine.SupplierId))
                    .Authorize();

            var deletedMedicine = _supplierStockService.Delete(id);

            return Ok(deletedMedicine);
        }

        /// <summary>
        /// Returns all supplier's offers from the system.
        /// </summary>
        /// <response code="200">Returns list of supplier's offers.</response>
        [Authorize(Roles = "Supplier")]
        [HttpGet("{supplierId}/offers")]
        public IActionResult GetSuppliersOffers(Guid supplierId)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierId))
                    .Authorize();

            return Ok(_supplierOfferService.ReadFor(supplierId));
        }

        /// <summary>
        /// Returns all supplier's offers for a pharmacy order.
        /// </summary>
        /// <response code="200">Returns list of supplier's offers for a pharmacy order.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("offers/pharmacy-order/{pharmacyOrderId}")]
        public IActionResult GetSuppliersOffersForPharmacyOrder(Guid pharmacyOrderId)
        {
            var pharmacyOrder = _pharmacyOrderService.TryToRead(pharmacyOrderId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyOrder.PharmacyId))
                .Authorize();
            return Ok(_supplierOfferService.ReadForPharmacyOrder(pharmacyOrderId));
        }

        /// <summary>
        /// Accepts an existing supplier offer for a pharmacy order.
        /// </summary>
        /// <response code="200">Accepted supplier offer.</response>
        /// <response code="404">Offer, order, medicine or user missing.</response>
        /// <response code="400">
        ///     Order offers deadline not yer expired or
        ///     The offer or order has already been handled or
        ///     The pharmacy admin is not the creator of the order.
        /// </response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("offers/{offerId}")]
        public IActionResult AcceptSupplierOffer(Guid offerId)
        {
            var offer = _supplierOfferService.Read(offerId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(offer.PharmacyOrder.PharmacyId))
                .And(UserSpecific.For(offer.PharmacyOrder.PharmacyAdminId))
                .Authorize();
            
            return Ok(_supplierOfferService.AcceptOffer(offerId, offer.PharmacyOrder.PharmacyAdminId));
        }

        /// <summary>
        /// Returns all supplier's offers from the system filtered by given status.
        /// </summary>
        /// <response code="200">Returns list of supplier's offers.</response>
        [Authorize(Roles = "Supplier")]
        [HttpGet("{supplierId}/offers/filter")]
        public IActionResult GetSuppliersOffersFilteredByStatus(Guid supplierId, OfferStatus? status)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierId))
                    .Authorize();

            return Ok(_supplierOfferService.ReadByStatusFor(supplierId, status));
        }

        /// <summary>
        /// Returns page of supplier's offers from the system filtered by given status.
        /// </summary>
        /// <response code="200">Returns page of supplier's offers.</response>
        [Authorize(Roles = "Supplier")]
        [HttpGet("{supplierId}/offers/filter/page")]
        public IActionResult GetSuppliersOffersFilteredByStatusPage(Guid supplierId, OfferStatus? status, [FromQuery] PageDTO page)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(supplierId))
                    .Authorize();

            return Ok(_supplierOfferService.ReadPageOfOffersByStatusFor(supplierId, status, page));
        }

        /// <summary>
        /// Creates a new supplier's offer for pharmacy order in the system.
        /// </summary>
        /// <response code="200">Returns created supplier's offer.</response>
        /// <response code="400">
        /// Unable to create supplier's offer because supplier already gave offer
        /// for given order or delivery deadline time is too early or
        /// orders deadline passed or supplier don't have enough medicines.
        /// </response>
        /// <response code="404">Given supplier or pharmacy order doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpPost("{supplierId}/offers")]
        public IActionResult CreateSupplierOffer(CreateSupplierOfferRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(request.SupplierId))
                    .Authorize();

            var offer = _mapper.Map<SupplierOffer>(request);
            _supplierOfferService.Create(offer);

            return Ok(offer);
        }

        /// <summary>
        /// Updates an existing supplier's offer for pharmacy order from the system.
        /// </summary>
        /// <response code="200">Returns updated supplier's offer.</response>
        /// <response code="400">
        /// Unable to update supplier's offer because delivery deadline time is too early or
        /// orders deadline passed.
        /// </response>
        /// <response code="404">Given offer doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpPut("{supplierId}/offers")]
        public IActionResult UpdateSupplierOffer(UpdateSupplierOfferRequest request)
        {
            var existingOffer = _supplierOfferService.TryToRead(request.Id);
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(existingOffer.SupplierId))
                    .Authorize();

            var offer = _mapper.Map<SupplierOffer>(request);
            var updatedOffer = _supplierOfferService.Update(offer);

            return Ok(updatedOffer);
        }

        /// <summary>
        /// Cancels an existing supplier's offer for pharmacy order from the system.
        /// </summary>
        /// <response code="200">Returns canceled supplier's offer.</response>
        /// <response code="400">
        /// Unable to cancel supplier's offer because datetime is after offers deadline or
        /// order is already processed.
        /// </response>
        /// <response code="404">Given offer doesn't exist.</response>
        [Authorize(Roles = "Supplier")]
        [HttpDelete("{supplierId}/offers/{offerId}")]
        public IActionResult CancelSupplierOffer(Guid offerId)
        {
            var existingOffer = _supplierOfferService.TryToRead(offerId);
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(AccountSpecific.For(existingOffer.SupplierId))
                    .Authorize();

            var canceledOffer = _supplierOfferService.CancelOffer(offerId);

            return Ok(canceledOffer);
        }
    }
}