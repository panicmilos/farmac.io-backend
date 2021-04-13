using AutoMapper;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_API.Contracts.Requests.WorkTimes;
using Farmacio_API.Contracts.Responses.Dermatologists;

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
        private readonly IMapper _mapper;

        public PharmaciesController(IPharmacyService pharmacyService, IPharmacistService pharmacistService
            , IDermatologistService dermatologistService, IAppointmentService appointmentService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService, IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            _pharmacistService = pharmacistService;
            _dermatologistService = dermatologistService;
            _mapper = mapper;
            _appointmentService = appointmentService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
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
        [HttpGet("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult GetDermatologist(Guid pharmacyId, Guid dermatologistId)
        {
            return Ok(_dermatologistService.ReadForPharmacy(pharmacyId, dermatologistId));
        }

        /// <summary>
        /// Add an existing dermatologist to the pharmacy.
        /// </summary>
        /// <response code="200">Added dermatologist.</response>
        /// <response code="404">Dermatologist or Pharmacy not found.</response>
        /// <response code="400">Dermatologist already employed in the Pharmacy, work time invalid or overlaps with and existing one.</response>
        [HttpPost("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult AddDermatologistToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTimeRequest workTime)
        {
            return Ok(_dermatologistService.AddToPharmacy(pharmacyId, dermatologistId, _mapper.Map<WorkTime>(workTime)));
        }
        
        /// <summary>
        /// Remove dermatologist from the pharmacy.
        /// </summary>
        /// <response code="200">Removed dermatologist.</response>
        /// <response code="404">Dermatologist not found.</response>
        /// <response code="400">Dermatologist not employed in the pharmacy.</response>
        [HttpDelete("{pharmacyId}/dermatologists/{dermatologistId}")]
        public IActionResult RemoveDermatologistFromPharmacy(Guid pharmacyId, Guid dermatologistId)
        {
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
        /// Creates a new pharmacy in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy.</response>
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
        [HttpPut]
        public IActionResult UpdatePharmacy(UpdatePharmacyRequest request)
        {
            var pharmacy = _mapper.Map<Pharmacy>(request);
            return Ok(_pharmacyService.Update(pharmacy));
        }

        /// <summary>
        /// Deletes pharmacy specified by id.
        /// </summary>
        /// <response code="200">Returns deleted pharmacy.</response>
        /// <response code="401">Unable to delete pharmacy because it does not exist in the system.</response>
        [HttpDelete("{id}")]
        public IActionResult DeletePharmacy(Guid id)
        {
            var deletedPharmacy = _pharmacyService.Delete(id);

            return Ok(deletedPharmacy);
        }
    }
}