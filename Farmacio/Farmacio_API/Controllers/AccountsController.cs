using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("accounts")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Changes account's password.
        /// </summary>
        /// <response code="200">Returns account.</response>
        /// <response code="400">Unable to change account's password because current password is wrong.</response>
        /// <response code="404">Unable to change account's password because account does not exist in the system.</response>
        [Authorize]
        [HttpPut("{id}/password")]
        public IActionResult ChangePassword(Guid id, ChangePasswordRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(AccountSpecific.For(id))
                                .Authorize();

            return Ok(_accountService.ChangePasswordFor(id, request.CurrentPassword, request.NewPassword));
        }
    }
}