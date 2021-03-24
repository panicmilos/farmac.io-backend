using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new patient in the system.
        /// </summary>
        /// <response code="200">Created patient.</response>
        /// <response code="401">Username or email is already taken.</response>
        [HttpPost("/create-patient")]
        public IActionResult CreatePatient(CreatePatientRequest request)
        {
            var patient = _mapper.Map<Account>(request);
            _accountService.Create(patient);

            return Ok(patient);
        }
    }
}