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
    public class ReadWithMostRecentPricesForTests : FarmacioTestBase
    {
        private readonly IPharmacyPriceListService _pharmacyPriceListService;

        public ReadWithMostRecentPricesForTests()
        {
            _pharmacyPriceListService = new PharmacyPriceListService(new Repository<PharmacyPriceList>(context));
        }

        [Fact]
        public void ReadWithMostRecentPricesFor_ForPharmacyIdExists_PharmacyIdsMatch()
        {
            var pharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3");

            var pharmacyPriceList = _pharmacyPriceListService.ReadWithMostRecentPricesFor(pharmacyId);

            Assert.NotNull(pharmacyPriceList);
            Assert.Equal(pharmacyId, pharmacyPriceList.PharmacyId);
        }

        [Fact]
        public void ReadWithMostRecentPricesFor_ForPharmacyIdNotExists_ReturnsNull()
        {
            var pharmacyId = new Guid("cb772db7-bc15-40a2-abe4-f1abe496dd00");

            var pharmacyPriceList = _pharmacyPriceListService.ReadWithMostRecentPricesFor(pharmacyId);

            Assert.Null(pharmacyPriceList);
        }

        [Fact]
        public void ReadWithMostRecentPricesFor_ForPharmacyIdExistsAndMedicinePricesForSameMedicineExist_ReturnsOnlyMostRecentPrices()
        {
            var pharmacyId = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3");

            var pharmacyPriceList = _pharmacyPriceListService.ReadWithMostRecentPricesFor(pharmacyId);

            Assert.NotNull(pharmacyPriceList);
            Assert.NotEmpty(pharmacyPriceList.MedicinePriceList);
            var areMedicinePricesUnique = pharmacyPriceList.MedicinePriceList.Distinct(
                                                  (firstMedicinePrice, secondMedicinePrice) =>
                                                      firstMedicinePrice.MedicineId == secondMedicinePrice.MedicineId)
                                              .Count() ==
                                          pharmacyPriceList.MedicinePriceList.Count;
            Assert.True(areMedicinePricesUnique);
        }
    }
}