using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Farmacio_Tests.IntegrationTests.PharmacyPriceListServiceTests
{
    public class TryToReadForTests : FarmacioTestBase
    {
        private readonly IPharmacyPriceListService _pharmacyPriceListService;

        public TryToReadForTests()
        {
            _pharmacyPriceListService = new PharmacyPriceListService(new Repository<PharmacyPriceList>(context));
        }

        [Fact]
        public void TryToReadFor_ForPharmacyIdExists_PharmacyIdsMatch()
        {
            var pharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3");

            var pharmacyPriceList = _pharmacyPriceListService.TryToReadFor(pharmacyId);

            Assert.NotNull(pharmacyPriceList);
            Assert.Equal(pharmacyId, pharmacyPriceList.PharmacyId);
        }

        [Fact]
        public void TryToReadFor_ForPharmacyIdNotExists_ThrowsException()
        {
            var pharmacyId = new Guid("cb772db7-bc15-40a2-abe4-f1abe496dd00");

            Assert.Throws<MissingEntityException>(() => _pharmacyPriceListService.TryToReadFor(pharmacyId));
        }
    }
}