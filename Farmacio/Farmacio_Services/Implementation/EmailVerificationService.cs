using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;

        public EmailVerificationService(ITokenProvider tokenProvider, IEmailDispatcher emailDispatcher, ITemplatesProvider templatesProvider)
        {
            _tokenProvider = tokenProvider;
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

            var verificationToken = _tokenProvider.GenerateEmailTokenFor(account);

            var verificationEmail = _templatesProvider.FromTemplate<Email>("EmailVerification", new { To = account.Email, Token = verificationToken });
            _emailDispatcher.Dispatch(verificationEmail);
        }
    }
}