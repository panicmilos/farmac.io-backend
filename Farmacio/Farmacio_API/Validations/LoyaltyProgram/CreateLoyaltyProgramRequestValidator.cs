using Farmacio_API.Contracts.Requests.LoyaltyPrograms;
using FluentValidation;

namespace Farmacio_API.Validations.LoyaltyProgram
{
    public class CreateLoyaltyProgramRequestValidator : AbstractValidator<CreateLoyaltyProgramRequest>
    {
        public CreateLoyaltyProgramRequestValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty().WithMessage("Loyalty program name must be provided.");

            RuleFor(request => request.MinPoints).GreaterThanOrEqualTo(0).WithMessage("Minimum points for loyalty program must be greater or equal to 0.");

            RuleFor(request => request.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Discount for loyalty program must be greater or equal to 0 and less or equal to 100.");
        }
    }
}