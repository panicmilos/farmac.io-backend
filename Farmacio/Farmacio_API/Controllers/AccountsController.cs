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

        public AccountsController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}