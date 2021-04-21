﻿using AutoMapper;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using Farmacio_Models.Domain;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PharmacyPriceListController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IMedicineService _medicineService;
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        private readonly IMapper _mapper;

        public PharmacyPriceListController(IPharmacyService pharmacyService, IMedicineService medicineService
            , IPharmacyPriceListService pharmacyPriceListService
            , IMapper mapper)
        {
            _pharmacyPriceListService = pharmacyPriceListService;
            _pharmacyService = pharmacyService;
            _medicineService = medicineService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Read an existing pharmacy price list by id.
        /// </summary>
        /// <response code="200">Read pharmacy price list.</response>
        /// <response code="404">Pharmacy or PharmacyPriceList not found.</response>
        [HttpGet("/pharmacy/{pharmacyId}/price-list/{pharmacyPriceListId}")]
        public IActionResult ReadPharmacyOrder(Guid pharmacyId, Guid pharmacyPriceListId)
        {
            _pharmacyService.TryToRead(pharmacyId);
            return Ok(_pharmacyPriceListService.TryToRead(pharmacyPriceListId));
        }
        
        /// <summary>
        /// Create a pharmacy price list.
        /// </summary>
        /// <response code="200">Created pharmacy price list.</response>
        /// <response code="404">Medicine or Pharmacy not found.</response>
        [HttpPost("/pharmacy/{pharmacyId}/price-list")]
        public IActionResult CreatePharmacyOrder(Guid pharmacyId, CreatePharmacyPriceListRequest pharmacyPriceListRequest)
        {
            var pharmacyPriceList = _mapper.Map<PharmacyPriceList>(pharmacyPriceListRequest);
            _pharmacyService.TryToRead(pharmacyPriceList.PharmacyId);
            pharmacyPriceList.MedicinePriceList.ForEach(orderedMedicine =>
                _medicineService.TryToRead(orderedMedicine.MedicineId));
            pharmacyPriceList.PharmacyId = pharmacyId;

            return Ok(_pharmacyPriceListService.Create(pharmacyPriceList));
        }
        
        /// <summary>
        /// Update an existing pharmacy price list.
        /// </summary>
        /// <response code="200">Updated pharmacy price list.</response>
        /// <response code="404">Medicine, PharmacyPriceList or Pharmacy not found.</response>
        [HttpPut("/pharmacy/{pharmacyId}/price-list")]
        public IActionResult UpdatePharmacyOrder(Guid pharmacyId, UpdatePharmacyPriceListRequest pharmacyPriceListsRequest)
        {
            var pharmacyPriceList = _mapper.Map<PharmacyPriceList>(pharmacyPriceListsRequest);
            _pharmacyService.TryToRead(pharmacyPriceList.PharmacyId);
            pharmacyPriceList.MedicinePriceList.ForEach(orderedMedicine =>
                _medicineService.TryToRead(orderedMedicine.MedicineId));
            pharmacyPriceList.PharmacyId = pharmacyId;

            return Ok(_pharmacyPriceListService.Update(pharmacyPriceList));
        }
    }
}
