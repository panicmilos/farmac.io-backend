using AutoMapper;
using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
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
        /// Returns all medicines from the system.
        /// </summary>
        /// <response code="200">Returns list of medicines.</response>
        [HttpGet]
        public IEnumerable<Medicine> GetMedicines()
        {
            return _medicineService.Read();
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

        /// <summary>
        /// Returns medicine specified by id.
        /// </summary>
        /// <response code="200">Returns medicine.</response>
        /// <response code="404">Unable to return medicine because it does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult GetMedicine(Guid id)
        {
            var medicine = _medicineService.ReadFullMedicine(id);
            if (medicine == null)
            {
                throw new MissingEntityException();
            }

            return Ok(medicine);
        }

        /// <summary>
        /// Returns all pharmacies where the medicine is available.
        /// </summary>
        /// <response code="200">Returns list of small pharmaciesOfMedicine objects</response>
        [HttpGet("{id}/pharmacies")]
        public IEnumerable<PharmaciesOfMedicineDTO> GetPharmacies(Guid id)
        {
            return _pharmacyService.MedicineInPharmacies(id);
        }

        /// <summary>
        /// Creates a new medicine in the system.
        /// </summary>
        /// <response code="200">Returns created medicine.</response>
        /// <response code="401">Unable to create medicine because code is already taken.</response>
        /// <response code="404">Unable to create medicine because replacement medicine does not.</response>
        [HttpPost]
        public IActionResult CreateMedicine(CreateMedicineRequest request)
        {
            var fullMedicineDto = _mapper.Map<FullMedicineDTO>(request);
            _medicineService.Create(fullMedicineDto);

            return Ok(fullMedicineDto);
        }

        /// <summary>
        /// Updates an existing medicine from the system.
        /// </summary>
        /// <response code="200">Returns updated medicine.</response>
        /// <response code="404">Unable to update medicine because it does not exist.</response>
        [HttpPut]
        public IActionResult UpdatePharmacyAdmin(UpdateMedicineRequest request)
        {
            var fullMedicineDto = _mapper.Map<FullMedicineDTO>(request);
            var updatedMedicine = _medicineService.Update(fullMedicineDto);

            return Ok(updatedMedicine);
        }

        /// <summary>
        /// Deletes medicine from the system.
        /// </summary>
        /// <response code="200">Returns deleted medicine.</response>
        /// <response code="404">Unable to delete medicine because it does not exist.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteMedicine(Guid id)
        {
            var deletedMedicine = _medicineService.Delete(id);

            return Ok(deletedMedicine);
        }
    }
}