using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("accounts")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPatientService _patientService;

        public AccountsController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new patient in the system.
        /// </summary>
        /// <response code="200">Created patient.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPost("create-patient")]
        public IActionResult CreatePatient(CreatePatientRequest request)
        {
            var patient = _mapper.Map<Account>(request);
            _patientService.Create(patient);

            return Ok(patient);
        }
    }
}