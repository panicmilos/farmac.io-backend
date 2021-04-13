using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
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
        private readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
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
            var supplier = _supplierService.Read(id);
            if (supplier == null)
            {
                throw new MissingEntityException();
            }

            return Ok(supplier);
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
    }
}