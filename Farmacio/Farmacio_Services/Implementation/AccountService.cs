using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System.Linq;

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
            _emailVerificationService.SendTo(account.Email);

            return createdAccount;
        }

        private bool IsUsernameTaken(string username)
        {
            var foundAccount = _repository.Read()
                                          .ToList()
                                          .FirstOrDefault(account => account.Username == username);

            return foundAccount != default;
        }

        private bool IsEmailTaken(string email)
        {
            var foundEmail = _repository.Read()
                                          .ToList()
                                          .FirstOrDefault(account => account.Email == email);

            return foundEmail != default;
        }
    }
}