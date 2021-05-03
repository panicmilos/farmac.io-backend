using Farmacio_API.Contracts.Requests.LoyaltyPoints;
using FluentValidation;

namespace Farmacio_API.Validations.LoyaltyPoints
{
    public class UpdateMedicinePointsRequestValidator : AbstractValidator<UpdateMedicinePointsRequest>
    {
        public UpdateMedicinePointsRequestValidator()
        {
            RuleFor(request => request.MedicineId).NotNull().NotEmpty().WithMessage("Medicine id must be provided.");

            RuleFor(request => request.Points).NotNull().GreaterThan(0).WithMessage("Points for medicine must be greater than 0.");
        }
    }
}