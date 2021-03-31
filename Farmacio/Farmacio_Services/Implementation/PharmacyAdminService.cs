using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PharmacyAdminService : AccountService, IPharmacyAdminService
    {
        private readonly IPharmacyService _pharmacyService;

        public PharmacyAdminService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService,
            IRepository<Account> repository) :
            base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.PharmacyAdmin).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.PharmacyAdmin ? account : null;
        }

        public override Account Create(Account account)
        {
            ValidatePharmacyId(account);

            return base.Create(account);
        }

        public override Account Update(Account account)
        {
            ValidatePharmacyId(account);
            var pharmacyAdmin = Read(account.Id);
            if (pharmacyAdmin == null)
            {
                throw new MissingEntityException();
            }

            pharmacyAdmin.User.FirstName = account.User.FirstName;
            pharmacyAdmin.User.LastName = account.User.LastName;
            pharmacyAdmin.User.PhoneNumber = account.User.PhoneNumber;
            pharmacyAdmin.User.PID = account.User.PID;
            pharmacyAdmin.User.DateOfBirth = account.User.DateOfBirth;

            pharmacyAdmin.User.Address.State = account.User.Address.State;
            pharmacyAdmin.User.Address.City = account.User.Address.City;
            pharmacyAdmin.User.Address.StreetName = account.User.Address.StreetName;
            pharmacyAdmin.User.Address.StreetNumber = account.User.Address.StreetNumber;
            pharmacyAdmin.User.Address.Lat = account.User.Address.Lat;
            pharmacyAdmin.User.Address.Lng = account.User.Address.Lng;

            return base.Update(pharmacyAdmin);
        }

        private void ValidatePharmacyId(Account account)
        {
            if (_pharmacyService.Read(((PharmacyAdmin)account.User).PharmacyId) == null)
                throw new MissingEntityException("Pharmacy with the provided id does not exist.");
        }
    }
}