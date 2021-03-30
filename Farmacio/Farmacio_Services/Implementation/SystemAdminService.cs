using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SystemAdminService : AccountService, ISystemAdminService
    {
        public SystemAdminService(IEmailVerificationService emailVerificationService, IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.SystemAdmin).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.SystemAdmin ? account : null;
        }

        public override Account Update(Account account)
        {
            var systemAdmin = Read(account.Id);
            if (systemAdmin == null)
            {
                throw new MissingEntityException();
            }

            systemAdmin.User.FirstName = account.User.FirstName;
            systemAdmin.User.LastName = account.User.LastName;
            systemAdmin.User.PhoneNumber = account.User.PhoneNumber;
            systemAdmin.User.PID = account.User.PID;
            systemAdmin.User.DateOfBirth = account.User.DateOfBirth;

            systemAdmin.User.Address.State = account.User.Address.State;
            systemAdmin.User.Address.City = account.User.Address.City;
            systemAdmin.User.Address.StreetName = account.User.Address.StreetName;
            systemAdmin.User.Address.StreetNumber = account.User.Address.StreetNumber;
            systemAdmin.User.Address.Lat = account.User.Address.Lat;
            systemAdmin.User.Address.Lng = account.User.Address.Lng;

            return base.Update(systemAdmin);
        }
    }
}