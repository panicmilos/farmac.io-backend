using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using System;
using Xunit;

namespace Farmacio_Tests.IntegrationTests.SupplierOfferServiceTests
{
    public class CancelOfferTests : FarmacioTestBase
    {
        private readonly ISupplierOfferService _supplierOfferService;
        private readonly IRepository<SupplierOffer> _supplierOfferRepository;
        private readonly ISupplierStockService _supplierStockService;

        public CancelOfferTests()
        {
            _supplierOfferRepository = new Repository<SupplierOffer>(context);
            _supplierStockService = new SupplierStockService(null, null, new Repository<SupplierMedicine>(context));

            _supplierOfferService = new SupplierOfferService(null, null, _supplierStockService, null, null, null, _supplierOfferRepository);
        }

        [Fact]
        public void CancelOffer_ThrowsMissingEntityException_BecauseSupplierOfferDoesntExist()
        {
            var notExistingSupplierOffer = new Guid("9f8aa491-aae7-4bfb-82ad-e06b929185d2");

            Action cancelAction = () => _supplierOfferService.CancelOffer(notExistingSupplierOffer);

            cancelAction.Should().Throw<MissingEntityException>();
        }

        [Fact]
        public void CancelOffer_ThrowsOrderIsAlreadyProcessedException_BecauseOrderIsProcessed()
        {
            var offerThatIsAlreadyProcessed = new Guid("a0913d33-0b11-42a5-ad2f-4b7b4ef16ce8");

            Action cancelAction = () => _supplierOfferService.CancelOffer(offerThatIsAlreadyProcessed);

            cancelAction.Should().Throw<OrderIsAlreadyProcessedException>();
        }

        [Fact]
        public void CancelOffer_BadLogicException_BecauseOffersDeadlineIsInPast()
        {
            var offerWhichDeadlinePassed = new Guid("72978073-01eb-4e6e-a623-1f5d41fda64d");

            Action cancelAction = () => _supplierOfferService.CancelOffer(offerWhichDeadlinePassed);

            cancelAction.Should().Throw<BadLogicException>();
        }

        [Fact]
        public void CancelOffer_ReturnsCanceledOffer()
        {
            using var transaction = context.Database.BeginTransaction();
            var existingOffer = new Guid("771ae913-8707-45e6-a4e6-3e71c21067f6");

            var canceledOffer = _supplierOfferService.CancelOffer(existingOffer);

            canceledOffer.Should().NotBeNull();
            canceledOffer.Active.Should().BeFalse();
            transaction.Rollback();
        }
    }
}