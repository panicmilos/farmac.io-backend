using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Validations.PharmacyMedicines;
using Farmacio_API.Validations.PharmacyPriceLists;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyOrders
{
    public class CreatePharmacyOrderRequestValidator : AbstractValidator<CreatePharmacyOrderRequest>
    {
        public CreatePharmacyOrderRequestValidator()
        {
            RuleFor(request => request.PharmacyAdminId).NotNull().NotEmpty()
                .WithMessage("Pharmacy administrator id must be provided.");
            RuleFor(request => request.OffersDeadline).NotNull().NotEmpty()
                .WithMessage("Offer deadline must be provided.");
            RuleFor(request => request.OrderedMedicines).NotNull().NotEmpty()
                .WithMessage("Ordered medicines list must be provided.");
            RuleForEach(request => request.OrderedMedicines).NotNull()
                .SetValidator(new CreatePharmacyMedicineRequestValidator())
                .WithMessage("Invalid ordered medicine.");
        }
    }
}