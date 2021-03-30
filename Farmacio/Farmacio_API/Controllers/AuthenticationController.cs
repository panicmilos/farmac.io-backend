using Farmacio_API.Contracts.Requests.Authentication;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest request)
        {
            var authenticatedUser = _authenticationService.Authenticate(request.Username, request.Password);

            return Ok(authenticatedUser);
        }
    }
}