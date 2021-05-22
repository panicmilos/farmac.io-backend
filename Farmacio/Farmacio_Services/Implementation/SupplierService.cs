using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class SupplierService : AccountService, ISupplierService
    {
        private readonly ICrudService<SupplierOffer> _supplierOffersService;
        private readonly ICrudService<SupplierMedicine> _supplierMedicineService;

        public SupplierService(
            IEmailVerificationService emailVerificationService,
            ICrudService<SupplierOffer> supplierOffersService,
            ICrudService<SupplierMedicine> supplierMedicineService,
            IAccountRepository repository) :
            base(emailVerificationService, repository)
        {
            _supplierOffersService = supplierOffersService;
            _supplierMedicineService = supplierMedicineService;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Supplier).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Supplier ? account : null;
        }

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("Supplier account not found.");
            return existingAccount;
        }

        public override Account Delete(Guid id)
        {
            var existingSupplierAccount = TryToRead(id);
            var existingSupplierUser = existingSupplierAccount.User as Supplier;

            var numberOfActiveSupplierOffers = _supplierOffersService.Read().Where(offer =>
                offer.SupplierId == existingSupplierAccount.Id &&
                offer.Status == OfferStatus.WaitingForAnswer).Count();

            if (numberOfActiveSupplierOffers > 0)
            {
                throw new BadLogicException("You cannot delete supplier because he has active offers.");
            }

            var supplierMedicinesIds = _supplierMedicineService.Read()
                .Where(supplierMedicine => supplierMedicine.SupplierId == existingSupplierAccount.Id)
                .Select(supplierMedicine => supplierMedicine.Id).ToList();

            _supplierMedicineService.Delete(supplierMedicinesIds);

            return base.Delete(id);
        }
    }
}