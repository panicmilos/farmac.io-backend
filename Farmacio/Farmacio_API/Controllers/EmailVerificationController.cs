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
        private readonly IAccountService _accountService;

        public EmailVerificationController(IEmailVerificationService emailVerificationService, IAccountService accountService)
        {
            _emailVerificationService = emailVerificationService;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult GetVerificationMail(string email)
        {
            var account = _accountService.ReadByEmail(email);
            _emailVerificationService.SendTo(account);

            return Ok();
        }

        [HttpPut]
        [EmailSecretAuthorization]
        public IActionResult VerifyAccount()
        {
            var accountId = new Guid(HttpContext.Request.Headers["accountId"]);
            var verifiedAccount = _accountService.Verify(accountId);

            return Ok(verifiedAccount);
        }
    }
}