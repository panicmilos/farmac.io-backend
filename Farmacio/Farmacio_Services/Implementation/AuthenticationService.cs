using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using System;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationService(IRepository<Account> accountRepository, ITokenProvider tokenProvider)
        {
            _accountRepository = accountRepository;
            _tokenProvider = tokenProvider;
        }

        public AuthenticatedUserDTO Authenticate(string username, string password)
        {
            var authenticatedAccount = _accountRepository
                .Read()
                .FirstOrDefault(account =>
                    account.Username == username &&
                    account.Password == CryptographyUtils.GetSaltedAndHashedPassword(password, account.Salt));

            if (authenticatedAccount == null)
            {
                throw new CredentialsDontMatchAnyAccountException("Given combination of username and password does not match any account.");
            }

            if (!authenticatedAccount.IsVerified)
            {
                throw new NotVerifiedAccountException("You have to verify account before being able to log in.");
            }

            var token = _tokenProvider.GenerateAuthTokenFor(authenticatedAccount);

            return new AuthenticatedUserDTO
            {
                Token = token
            };
        }
    }
}