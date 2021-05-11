using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using GlobalExceptionHandler.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Farmacio_Tests.UnitTests.SupplierOfferServiceTests
{
    public class CancelOfferTests
    {
        private readonly ISupplierOfferService _supplierOfferService;
        private readonly Mock<IRepository<SupplierOffer>> _supplierOfferRepository;
        private readonly Mock<ISupplierStockService> _supplierStockService;

        public CancelOfferTests()
        {
            _supplierOfferRepository = new Mock<IRepository<SupplierOffer>>();
            _supplierStockService = new Mock<ISupplierStockService>();

            _supplierOfferService = new SupplierOfferService(null, null, _supplierStockService.Object, null, null, null, _supplierOfferRepository.Object);
        }

        [Fact]
        public void CancelOffer_ThrowsMissingEntityException_BecauseSupplierOfferDoesntExist()
        {
            _supplierOfferRepository.Setup(repository => repository.Read(It.IsAny<Guid>())).Returns((Guid id) => null);

            Action cancelAction = () => _supplierOfferService.CancelOffer(new Guid("9f8aa491-aae7-4bfb-82ad-e06b929185d2"));

            cancelAction.Should().Throw<MissingEntityException>();
            _supplierOfferRepository.Verify(repository => repository.Read(It.IsAny<Guid>()), Times.Once);
            _supplierStockService.Verify(service => service.ReadMedicineFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _supplierStockService.Verify(service => service.Update(It.IsAny<SupplierMedicine>()), Times.Never);
        }

        [Fact]
        public void CancelOffer_ThrowsOrderIsAlreadyProcessedException_BecauseOrderIsProcessed()
        {
            _supplierOfferRepository.Setup(repository => repository.Read(It.IsAny<Guid>())).Returns((Guid id) => new SupplierOffer
            {
                Id = id,
                PharmacyOrder = new PharmacyOrder
                {
                    IsProcessed = true
                }
            });

            Action cancelAction = () => _supplierOfferService.CancelOffer(new Guid("a0913d33-0b11-42a5-ad2f-4b7b4ef16ce8"));

            cancelAction.Should().Throw<OrderIsAlreadyProcessedException>();
            _supplierOfferRepository.Verify(repository => repository.Read(It.IsAny<Guid>()), Times.Once);
            _supplierStockService.Verify(service => service.ReadMedicineFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _supplierStockService.Verify(service => service.Update(It.IsAny<SupplierMedicine>()), Times.Never);
            _supplierOfferRepository.Verify(repository => repository.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CancelOffer_BadLogicException_BecauseOffersDeadlineIsInPast()
        {
            _supplierOfferRepository.Setup(repository => repository.Read(It.IsAny<Guid>())).Returns((Guid id) => new SupplierOffer
            {
                Id = id,
                PharmacyOrder = new PharmacyOrder
                {
                    IsProcessed = false,
                    OffersDeadline = DateTime.Now.AddDays(-1)
                }
            });

            Action cancelAction = () => _supplierOfferService.CancelOffer(new Guid("a0913d33-0b11-42a5-ad2f-4b7b4ef16ce8"));

            cancelAction.Should().Throw<BadLogicException>();
            _supplierOfferRepository.Verify(repository => repository.Read(It.IsAny<Guid>()), Times.Once);
            _supplierStockService.Verify(service => service.ReadMedicineFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _supplierStockService.Verify(service => service.Update(It.IsAny<SupplierMedicine>()), Times.Never);
            _supplierOfferRepository.Verify(repository => repository.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void CancelOffer_ReturnsCanceledOffer()
        {
            var supplierOffer = new SupplierOffer
            {
                PharmacyOrder = new PharmacyOrder
                {
                    IsProcessed = false,
                    OffersDeadline = DateTime.Now.AddDays(2),
                    OrderedMedicines = new List<OrderedMedicine>
                    {
                        new OrderedMedicine
                        {
                            MedicineId = new Guid("f42007bc-eba6-4895-816c-e3bf4e6ef9e6"),
                            Quantity = 3
                        },
                        new OrderedMedicine
                        {
                             MedicineId = new Guid("2958fa04-e404-45f8-92b4-c0b060bf40ce"),
                             Quantity = 1
                        }
                    }
                },
                SupplierId = new Guid("771ae913-8707-45e6-a4e6-3e71c21067f6"),
                Active = true
            };

            _supplierOfferRepository.Setup(repository => repository.Read(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                supplierOffer.Id = id;
                return supplierOffer;
            });
            _supplierOfferRepository.Setup(repository => repository.Delete(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                supplierOffer.Id = id;
                supplierOffer.Active = false;
                return supplierOffer;
            });

            _supplierStockService.Setup(service => service.ReadMedicineFor(It.IsAny<Guid>(), new Guid("f42007bc-eba6-4895-816c-e3bf4e6ef9e6"))).Returns((Guid supplierId, Guid medicineId) =>
            {
                return new SupplierMedicine
                {
                    SupplierId = supplierId,
                    MedicineId = medicineId,
                    Quantity = 4
                };
            });

            _supplierStockService.Setup(service => service.ReadMedicineFor(It.IsAny<Guid>(), new Guid("2958fa04-e404-45f8-92b4-c0b060bf40ce"))).Returns((Guid supplierId, Guid medicineId) =>
            {
                return new SupplierMedicine
                {
                    SupplierId = supplierId,
                    MedicineId = medicineId,
                    Quantity = 2
                };
            });

            _supplierStockService.Setup(service => service.Update(It.IsAny<SupplierMedicine>())).Returns((SupplierMedicine supplierMedicine) => supplierMedicine);

            var canceledOffer = _supplierOfferService.CancelOffer(new Guid("a0913d33-0b11-42a5-ad2f-4b7b4ef16ce8"));

            canceledOffer.Should().NotBeNull();
            canceledOffer.Active.Should().BeFalse();
            _supplierOfferRepository.Verify(repository => repository.Read(It.IsAny<Guid>()), Times.Once);
            _supplierStockService.Verify(service => service.ReadMedicineFor(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(2));
            _supplierStockService.Verify(service => service.Update(It.IsAny<SupplierMedicine>()), Times.Exactly(2));
            _supplierOfferRepository.Verify(repository => repository.Delete(It.IsAny<Guid>()), Times.Once);
        }
    }
}