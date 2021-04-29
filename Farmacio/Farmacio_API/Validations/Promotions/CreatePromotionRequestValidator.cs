using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Contracts.Requests.Promotions;
using Farmacio_API.Validations.PharmacyMedicines;
using Farmacio_API.Validations.PharmacyPriceLists;
using FluentValidation;

namespace Farmacio_API.Validations.Promotions
{
    public class CreatePromotionRequestValidator : AbstractValidator<CreatePromotionRequest>
    {
        public CreatePromotionRequestValidator()
        {
            RuleFor(request => request.From).NotNull().WithMessage("From date must be provided.");
            RuleFor(request => request.To).NotNull().WithMessage("To date must be provided.");
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("Pharmacy id must be provided.");
            RuleFor(request => request.Discount).NotNull().WithMessage("Invalid ordered medicine.");
        }
    }
}