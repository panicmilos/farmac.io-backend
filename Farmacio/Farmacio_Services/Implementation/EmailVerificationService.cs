using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;

        public EmailVerificationService(ITokenService tokenService, IEmailDispatcher emailDispatcher, ITemplatesProvider templatesProvider)
        {
            _tokenService = tokenService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
        }

        public void SendTo(Account account)
        {
            if (account == null)
            {
                throw new MissingEntityException($"Account does not exist in the system.");
            }
            if (account.IsVerified)
            {
                throw new BadLogicException("Account is already verified.");
            }

            var verificationToken = _tokenService.GenerateEmailTokenFor(account);

            var verificationEmail = _templatesProvider.FromTemplate<Email>("EmailVerification", new { To = account.Email, Token = verificationToken });
            _emailDispatcher.Dispatch(verificationEmail);
        }
    }
}