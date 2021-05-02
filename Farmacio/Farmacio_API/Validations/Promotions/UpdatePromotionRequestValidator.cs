using Farmacio_API.Contracts.Requests.Promotions;
using FluentValidation;

namespace Farmacio_API.Validations.Promotions
{
    public class UpdatePromotionRequestValidator : AbstractValidator<UpdatePromotionRequest>
    {
        public UpdatePromotionRequestValidator()
        {
            RuleFor(request => request.Id).NotNull().WithMessage("Promotion id must be provided.");
            RuleFor(request => request.From).NotNull().WithMessage("From date must be provided.");
            RuleFor(request => request.To).NotNull().WithMessage("To date must be provided.");
            RuleFor(request => request.Discount).NotNull().WithMessage("Invalid ordered medicine.");
        }
    }
}