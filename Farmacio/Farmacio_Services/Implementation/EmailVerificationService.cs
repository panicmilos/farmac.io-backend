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
        private readonly IRepository<Account> _accountRepository;

        public EmailVerificationService(ITokenService tokenService, IEmailDispatcher emailDispatcher, ITemplatesProvider templatesProvider, IRepository<Account> accountRepository)
        {
            _tokenService = tokenService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _accountRepository = accountRepository;
        }

        public void SendTo(string email)
        {
            var account = GetAccountByEmail(email);
            if (account == null)
            {
                throw new MissingEntityException($"Account with {email} does not exist in the system.");
            }
            if (account.IsVerified)
            {
                throw new BadLogicException("Account is already verified.");
            }

            var verificationToken = _tokenService.GenerateEmailTokenFor(account);

            var verificationEmail = _templatesProvider.FromTemplate<Email>("EmailVerification", new { To = email, Token = verificationToken });
            _emailDispatcher.Dispatch(verificationEmail);
        }

        public void Verify(Guid accountId)
        {
            var account = _accountRepository.Read(accountId);
            if (account == null)
            {
                throw new MissingEntityException($"Account does not exist in the system.");
            }
            if (account.IsVerified)
            {
                throw new BadLogicException("Account is already verified.");
            }

            account.IsVerified = true;
            _accountRepository.Update(account);
        }

        private Account GetAccountByEmail(string email)
        {
            return _accountRepository.Read()
                                     .ToList()
                                     .FirstOrDefault(account => account.Email == email);
        }
    }
}