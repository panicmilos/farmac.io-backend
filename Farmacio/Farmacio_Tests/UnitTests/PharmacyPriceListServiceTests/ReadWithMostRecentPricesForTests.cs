using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Farmacio_Tests.UnitTests.PharmacyPriceListServiceTests
{
    public class ReadWithMostRecentPricesForTests
    {
        private readonly IPharmacyPriceListService _pharmacyPriceListService;
        private readonly Mock<IRepository<PharmacyPriceList>> _pharmacyPriceListRepository;

        public ReadWithMostRecentPricesForTests()
        {
            _pharmacyPriceListRepository = new Mock<IRepository<PharmacyPriceList>>();
            _pharmacyPriceListRepository.Setup(repository => repository.Read()).Returns(new List<PharmacyPriceList>
            {
                new PharmacyPriceList
                {
                    PharmacyId = new Guid("cb772db7-bc15-40a2-abe4-f1abe496dddb"),
                    ConsultationPrice = 200f,
                    ExaminationPrice = 300f,
                    MedicinePriceList = new List<MedicinePrice>
                    {
                        new MedicinePrice
                        {
                            MedicineId = new Guid("8333525b-6760-43c0-8276-07c1a6d1df8e"),
                            Price = 30f,
                            ActiveFrom = new DateTime(2020, 12, 23, 6, 0, 0)
                        },
                        new MedicinePrice
                        {
                            MedicineId = new Guid("8333525b-6760-43c0-8276-07c1a6d1df8e"),
                            Price = 40f,
                            ActiveFrom = new DateTime(2020, 12, 23, 7, 0, 0)
                        },
                        new MedicinePrice
                        {
                            MedicineId = new Guid("b321bb5b-db8a-4a77-a20a-672727e13b87"),
                            Price = 50f,
                            ActiveFrom = new DateTime(2020, 12, 23, 6, 0, 0)
                        },
                    }
                },
                new PharmacyPriceList
                {
                    PharmacyId = new Guid("3494a92b-cc8c-4fb4-9da8-c2c8f3621782"),
                }
            });
            _pharmacyPriceListService = new PharmacyPriceListService(_pharmacyPriceListRepository.Object);
        }

        [Fact]
        public void ReadWithMostRecentPricesFor_ForPharmacyIdExists_PharmacyIdsMatch()
        {
            var pharmacyId = new Guid("cb772db7-bc15-40a2-abe4-f1abe496dddb");

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
            var pharmacyId = new Guid("cb772db7-bc15-40a2-abe4-f1abe496dddb");

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