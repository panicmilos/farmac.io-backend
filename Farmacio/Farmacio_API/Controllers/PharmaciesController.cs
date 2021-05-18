using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.WorkTimes;
using Farmacio_API.Contracts.Responses.Dermatologists;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_API.Controllers
{
    [Route("pharmacies")]
    [ApiController]
    [Produces("application/json")]
    public class PharmaciesController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacistService _pharmacistService;
        private readonly IDermatologistService _dermatologistService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IPharmacyStockService _pharmacyStockService;
        private readonly IMedicineService _medicineService;
        private readonly IAbsenceRequestService _absenceRequestService;
        private readonly IMapper _mapper;

        public PharmaciesController(IPharmacyService pharmacyService,
            IPharmacistService pharmacistService,
            IDermatologistService dermatologistService,
            IAppointmentService appointmentService,
            IDermatologistWorkPlaceService dermatologistWorkPlaceService,
            IPharmacyStockService pharmacyStockService,
            IMedicineService medicineService,
            IAbsenceRequestService absenceRequestService,
            IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            _pharmacistService = pharmacistService;
            _dermatologistService = dermatologistService;
            _appointmentService = appointmentService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _pharmacyStockService = pharmacyStockService;
            _medicineService = medicineService;
            _absenceRequestService = absenceRequestService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pharmacies from the system.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet]
        public IEnumerable<Pharmacy> ReadPharmacies()
        {
            return _pharmacyService.Read();
        }

        /// <summary>
        /// Returns all pharmacies from the system for home page with less information.
        /// </summary>
        /// <response code="200">Returns list of small pharmacy objects.</response>
        [HttpGet("home")]
        public IEnumerable<SmallPharmacyDTO> ReadForHomePage()
        {
            return _pharmacyService.ReadForHomePage();
        }

        /// <summary>
        /// Returns pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns pharmacy.</response>
        /// <response code="401">Unable to return pharmacy because it does not exist in the system.</response>
        [HttpGet("{id}")]
        public IActionResult ReadPharmacy(Guid id)
        {
            var pharmacy = _pharmacyService.Read(id);
            if (pharmacy == null)
            {
                throw new MissingEntityException();
            }

            return Ok(pharmacy);
        }

        /// <summary>
        /// Reads all existing pharmacists employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read pharmacists from the pharmacy.</response>
        [HttpGet("{pharmacyId}/pharmacists")]
        public IActionResult GetPharmacists(Guid pharmacyId)
        {
            return Ok(_pharmacistService.ReadForPharmacy(pharmacyId));
        }

        /// <summary>
        /// Search all existing pharmacists in the pharmacy.
        /// </summary>
        /// <response code="200">Searched pharmacists.</response>
        [HttpGet("{pharmacyId}/pharmacists/search")]
        public IActionResult SearchPharmacists(Guid pharmacyId, string name)
        {
            return Ok(_pharmacistService.SearchByNameForPharmacy(pharmacyId, name));
        }

        /// <summary>
        /// Reads an existing pharmacist employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read pharmacist from the pharmacy.</response>
        [Authorize(Roles = "PharmacyAdmin, SystemAdmin, Patient")]
        [HttpGet("{pharmacyId}/pharmacists/{pharmacistId}")]
        public IActionResult GetPharmacist(Guid pharmacyId, Guid pharmacistId)
        {
            return Ok(_pharmacistService.ReadForPharmacy(pharmacyId, pharmacistId));
        }

        /// <summary>
        /// Reads all existing dermatologists employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read dermatologists from the pharmacy.</response>
        [HttpGet("{pharmacyId}/dermatologists")]
        public IActionResult GetDermatologists(Guid pharmacyId)
        {
            return Ok(_dermatologistService.ReadForPharmacy(pharmacyId));
        }

        /// <summary>
        /// Search all existing dermatologists in the pharmacy.
        /// </summary>
        /// <response code="200">Searched dermatologists.</response>
        [HttpGet("{pharmacyId}/dermatologists/search")]
        public IActionResult SearchDermatologists(Guid pharmacyId, string name)
        {
            return Ok(_dermatologistService.SearchByNameForPharmacy(pharmacyId, name));
        }

        /// <summary>
        /// Reads all existing dermatologists employed in the pharmacy with their work places.
        /// </summary>
        /// <response code="200">Read dermatologists with their work places from the pharmacy.</response>
        [HttpGet("{pharmacyId}/dermatologists/with-work-places")]
        public IActionResult GetDermatologistsWithWorkPlaces(Guid pharmacyId)
        {
            return Ok(_dermatologistService.ReadForPharmacy(pharmacyId).Select(dermatologistAccount =>
                new DermatologistWithWorkPlacesResponse
                {
                    DermatologistAccount = dermatologistAccount,
                    WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
                }));
        }

        /// <summary>
        /// Search all existing dermatologists in the pharmacy with their work places.
        /// </summary>
        /// <response code="200">Searched dermatologists with their work places.</response>
        [HttpGet("{pharmacyId}/dermatologists/with-work-places/search")]
        public IActionResult SearchDermatologistsWithWorkPlaces(Guid pharmacyId, string name)
        {
            return Ok(_dermatologistService.SearchByNameForPharmacy(pharmacyId, name).Select(dermatologistAccount =>
                new DermatologistWithWorkPlacesResponse
                {
                    DermatologistAccount = dermatologistAccount,
                    WorkPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(dermatologistAccount.User.Id)
                }));
        }

        /// <summary>
        /// Reads an existing dermatologist employed in the pharmacy.
        /// </summary>
        /// <response code="200">Read dermatologist from the pharmacy.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult GetDermatologist(Guid pharmacyId, Guid dermatologistId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_dermatologistService.ReadForPharmacy(pharmacyId, dermatologistId));
        }

        /// <summary>
        /// Reads an existing absence requests in the pharmacy.
        /// </summary>
        /// <response code="200">Read absence requests.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("{pharmacyId}/absence-requests")]
        public IActionResult GetAbsenceRequestsForPharmacy(Guid pharmacyId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_absenceRequestService.ReadFor(pharmacyId));
        }

        /// <summary>
        /// Reads an existing absence requests page in the pharmacy.
        /// </summary>
        /// <response code="200">Read absence requests page.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpGet("{pharmacyId}/absence-requests/page")]
        public IActionResult GetAbsenceRequestsPageForPharmacy(Guid pharmacyId, int number, int size)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_absenceRequestService.ReadPageFor(pharmacyId, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }

        /// <summary>
        /// Reads all medicines that are in stock in the pharmacy.
        /// </summary>
        /// <response code="200">Read medicines that are in stock in the pharmacy.</response>
        [HttpGet("{pharmacyId}/medicines-in-stock")]
        public IActionResult GetMedicinesInStock(Guid pharmacyId)
        {
            return Ok(_pharmacyStockService.ReadForPharmacyInStock(pharmacyId));
        }

        /// <summary>
        /// Searches through all medicines that are in stock in the pharmacy.
        /// </summary>
        /// <response code="200">Searched medicines that are in stock in the pharmacy.</response>
        [HttpGet("{pharmacyId}/medicines-in-stock/search")]
        public IActionResult SearchMedicinesInStock(Guid pharmacyId, [FromQuery] string name)
        {
            return Ok(_pharmacyStockService.SearchForPharmacyInStock(pharmacyId, name));
        }

        /// <summary>
        /// Add an existing medicine to the pharmacy.
        /// </summary>
        /// <response code="200">Added pharmacy medicine.</response>
        /// <response code="404">Medicine or Pharmacy not found.</response>
        /// <response code="400">Pharmacy-Medicine already exists in the Pharmacy.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("{pharmacyId}/medicines")]
        public IActionResult AddMedicineToPharmacy(Guid pharmacyId, CreatePharmacyMedicineRequest pharmacyMedicineRequest)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            var pharmacyMedicine = _mapper.Map<PharmacyMedicine>(pharmacyMedicineRequest);
            pharmacyMedicine.PharmacyId = pharmacyId;
            _pharmacyService.TryToRead(pharmacyMedicine.PharmacyId);
            _medicineService.TryToRead(pharmacyMedicine.MedicineId);
            return Ok(_pharmacyStockService.Create(pharmacyMedicine));
        }

        /// <summary>
        /// Remove an existing medicine from the pharmacy.
        /// </summary>
        /// <response code="200">Removed pharmacy medicine.</response>
        /// <response code="404">Pharmacy-Medicine not found.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpDelete("{pharmacyId}/medicines/{pharmacyMedicineId}")]
        public IActionResult RemoveMedicineFromPharmacy(Guid pharmacyId, Guid pharmacyMedicineId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            var existingPharmacyMedicine = _pharmacyStockService.TryToRead(pharmacyMedicineId);
            if (existingPharmacyMedicine.PharmacyId != pharmacyId)
                throw new UnauthorizedAccessException("The given pharmacy medicine does not belong to the provided pharmacy.");
            return Ok(_pharmacyStockService.Delete(pharmacyMedicineId));
        }

        /// <summary>
        /// Add an existing dermatologist to the pharmacy.
        /// </summary>
        /// <response code="200">Added dermatologist.</response>
        /// <response code="404">Dermatologist or Pharmacy not found.</response>
        /// <response code="400">Dermatologist already employed in the Pharmacy, work time invalid or overlaps with and existing one.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult AddDermatologistToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTimeRequest workTime)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_dermatologistService.AddToPharmacy(pharmacyId, dermatologistId, _mapper.Map<WorkTime>(workTime)));
        }

        /// <summary>
        /// Remove dermatologist from the pharmacy.
        /// </summary>
        /// <response code="200">Removed dermatologist.</response>
        /// <response code="404">Dermatologist not found.</response>
        /// <response code="400">Dermatologist not employed in the pharmacy.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpDelete("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult RemoveDermatologistFromPharmacy(Guid pharmacyId, Guid dermatologistId)
        {
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(pharmacyId))
                .Authorize();
            return Ok(_dermatologistService.RemoveFromPharmacy(pharmacyId, dermatologistId));
        }

        /// <summary>
        /// Reads all existing appointments in the pharmacy.
        /// </summary>
        /// <response code="200">Read dermatologists from the pharmacy.</response>
        [HttpGet("{pharmacyId}/dermatologists/appointments")]
        public IActionResult GetDermatologistAppointmentsForPharmacy(Guid pharmacyId)
        {
            return Ok(_appointmentService.ReadForDermatologistsInPharmacy(pharmacyId));
        }

        /// <summary>
        /// Reads existing dermatologists appointments page in the pharmacy.
        /// </summary>
        /// <response code="200">Read dermatologists appointments page from the pharmacy.</response>
        [HttpGet("{pharmacyId}/dermatologists/appointments/page")]
        public IActionResult GetDermatologistAppointmentsPageForPharmacy(Guid pharmacyId, int number, int size)
        {
            return Ok(_appointmentService.ReadPageForDermatologistsInPharmacy(pharmacyId, new PageDTO
            {
                Number = number,
                Size = size
            }));
        }

        /// <summary>
        /// Creates a new pharmacy in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpPost]
        public IActionResult CreatePharmacy(CreatePharmacyRequest request)
        {
            var pharmacy = _mapper.Map<Pharmacy>(request);
            _pharmacyService.Create(pharmacy);

            return Ok(pharmacy);
        }

        /// <summary>
        /// Updates an existing pharmacy in the system.
        /// </summary>
        /// <response code="200">Updated pharmacy.</response>
        [Authorize(Roles = "SystemAdmin, PharmacyAdmin")]
        [HttpPut]
        public IActionResult UpdatePharmacy(UpdatePharmacyRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(IsPharmacyAdmin.Of(request.Id))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            var pharmacy = _mapper.Map<Pharmacy>(request);
            return Ok(_pharmacyService.Update(pharmacy));
        }

        /// <summary>
        /// Deletes pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns deleted pharmacy.</response>
        /// <response code="401">Unable to delete pharmacy because it does not exist in the system.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacy(Guid id)
        {
            var deletedPharmacy = _pharmacyService.Delete(id);

            return Ok(deletedPharmacy);
        }

        /// <summary>
        /// Returns pharmacies that contains given params.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet("search")]
        public IEnumerable<SmallPharmacyDTO> SearchPharmacies([FromQuery] PharmacySearchParams searchParams)
        {
            return _pharmacyService.ReadBy(searchParams);
        }

        /// <summary>
        /// Returns pharmacies that have available pharmacists in chosen time.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("available")]
        public IActionResult SearchAvailableForConsultations([FromQuery] SearhSortParamsForAppointments searchParams)
        {
            var pharmacists = _appointmentService.ReadPharmacistsForAppointment(_pharmacistService.Read(), searchParams);
            return Ok(_pharmacyService.GetPharmaciesOfPharmacists(pharmacists.ToList(), searchParams));
        }

        /// <summary>
        /// Returns available pharmacist from pharmacy.
        /// </summary>
        /// <response code="200">Returns list of pharmacists.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("available/{pharmacyId}/pharmacists")]
        public IActionResult GetFreePharmacist(Guid pharmacyId, [FromQuery] SearhSortParamsForAppointments searchParams)
        {
            _pharmacyService.TryToRead(pharmacyId);

            var pharmacists = _appointmentService.ReadPharmacistsForAppointment(_pharmacistService.Read(), searchParams).Where(pharmacistAccount =>
            {
                var pharmacist = pharmacistAccount.User as Pharmacist;
                return pharmacist.PharmacyId == pharmacyId;
            }).Select(pharmacistAccount => (Pharmacist)pharmacistAccount.User);

            return Ok(_pharmacistService.SortByGrade(pharmacists.ToList(), searchParams));
        }

        /// <summary>
        /// Returns first N pages of pharmacies that contains given params.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [HttpGet("search/pages-to")]
        public IEnumerable<SmallPharmacyDTO> SearchPharmaciesPagesTo([FromQuery] PharmacySearchParams searchParams, [FromQuery] PageDTO pageDTO)
        {
            return _pharmacyService.ReadPagesToBy(searchParams, pageDTO);
        }

        /// <summary>
        /// Returns pharmacies that have available pharmacists in chosen time for n-th page.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("available/page")]
        public IActionResult SearchAvailableForConsultationsPagesTo([FromQuery] SearhSortParamsForAppointments searchParams, [FromQuery] PageDTO pageDTO)
        {
            var pharmacists = _appointmentService.ReadPharmacistsForAppointment(_pharmacistService.Read(), searchParams);
            return Ok(_pharmacyService.GetPharmaciesOfPharmacistsPagesTo(pharmacists.ToList(), searchParams, pageDTO));
        }

        /// <summary>
        /// Returns all pharmacies from the system for home page with less information for n pages.
        /// </summary>
        /// <response code="200">Returns list of small pharmacy objects.</response>
        [HttpGet("page-to")]
        public IEnumerable<SmallPharmacyDTO> ReadForHomePage([FromQuery] PageDTO pageDTO)
        {
            return _pharmacyService.ReadAllPagesTo(pageDTO).Select(pharmacy => new SmallPharmacyDTO
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                Description = pharmacy.Description,
                Address = pharmacy.Address,
                AverageGrade = pharmacy.AverageGrade
            });
        }
    }
}