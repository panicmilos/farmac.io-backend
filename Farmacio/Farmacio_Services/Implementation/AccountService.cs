using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Repositories.Contracts;

namespace Farmacio_Services.Implementation
{
    public class AccountService : CrudService<Account>, IAccountService
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public AccountService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) :
            base(repository)
        {
            _emailVerificationService = emailVerificationService;
        }

        public override Account Create(Account account)
        {
            if (IsUsernameTaken(account.Username))
            {
                throw new BadLogicException("Username is already taken.");
            }

            if (IsEmailTaken(account.Email))
            {
                throw new BadLogicException("Email is already taken.");
            }

            account.Salt = CryptographyUtils.GetRandomSalt();
            account.Password = CryptographyUtils.GetSaltedAndHashedPassword(account.Password, account.Salt);
            var createdAccount = base.Create(account);
            _emailVerificationService.SendTo(createdAccount);

            return createdAccount;
        }

        public Account ReadByEmail(string email)
        {
            var foundEmail = _repository.Read()
                                        .FirstOrDefault(account => account.Email == email);

            return foundEmail;
        }

        public Account Verify(Guid accountId)
        {
            var account = _repository.Read(accountId);
            if (account == null)
            {
                throw new MissingEntityException($"Account does not exist in the system.");
            }
            if (account.IsVerified)
            {
                throw new BadLogicException("Account is already verified.");
            }

            account.IsVerified = true;
            return _repository.Update(account);
        }

        public IEnumerable<Account> SearchByName(string name)
        {
            return Read().Where(a =>
                name == null ||
                a.User.FirstName.ToLower().Contains(name.ToLower()) ||
                a.User.LastName.ToLower().Contains(name.ToLower()));
        }

        private bool IsUsernameTaken(string username)
        {
            var foundAccount = _repository.Read()
                                          .FirstOrDefault(account => account.Username == username);

            return foundAccount != default;
        }

        private bool IsEmailTaken(string email)
        {
            var foundEmail = ReadByEmail(email);

            return foundEmail != default;
        }
    }
}