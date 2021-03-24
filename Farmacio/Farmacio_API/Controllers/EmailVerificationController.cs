using Farmacio_API.Attributes;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("email-verification")]
    [Produces("application/json")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public EmailVerificationController(IEmailVerificationService emailVerificationService)
        {
            _emailVerificationService = emailVerificationService;
        }

        [HttpGet]
        public IActionResult GetVerificationMail(string email)
        {
            _emailVerificationService.SendTo(email);

            return Ok();
        }

        [HttpPut]
        [EmailSecretAuthorization]
        public IActionResult VerifyAccount()
        {
            var accountId = new Guid(HttpContext.Request.Headers["accountId"]);
            _emailVerificationService.Verify(accountId);

            return Ok();
        }
    }
}