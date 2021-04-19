using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;
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
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
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
            return Ok(_accountService.ChangePasswordFor(id, request.CurrentPassword, request.NewPassword));
        }
    }
}