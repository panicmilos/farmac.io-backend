using AutoMapper;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [Route("medicines")]
    [ApiController]
    [Produces("application/json")]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IPharmacyService _pharmacyService;
        private readonly IMapper _mapper;

        public MedicinesController(IMedicineService medicineService, IPharmacyService pharmacyService, IMapper mapper)
        {
            _medicineService = medicineService;
            _pharmacyService = pharmacyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all medicines from the system for home page with less information.
        /// </summary>
        /// <response code="200">Returns list of small medicines objects.</response>
        [HttpGet("home")]
        public IEnumerable<SmallMedicineDTO> ReadForHomePage()
        {
            return _medicineService.ReadForDisplay();
        }

        [HttpGet("getPharmacies/{id}")]
        public IEnumerable<PharmaciesOfMedicineDTO> GetPharmacies(Guid id)
        {
            return _pharmacyService.MedicineInPharmacies(id);
        }
    }
}