using Farmacio_API.Contracts.Requests.LoyaltyPoints;
using FluentValidation;

namespace Farmacio_API.Validations.LoyaltyPoints
{
    public class UpdateLoyaltyPointsRequestValidator : AbstractValidator<UpdateLoyaltyPointsRequest>
    {
        public UpdateLoyaltyPointsRequestValidator()
        {
            RuleFor(request => request.ConsultationPoints).NotNull().GreaterThanOrEqualTo(0).WithMessage("Points for consultation must be greater or request to 0.");

            RuleFor(request => request.ExaminationPoints).NotNull().GreaterThanOrEqualTo(0).WithMessage("Points for examination must be greater or request to 0.");

            RuleFor(request => request.MedicinePointsList).NotNull().WithMessage("Medicine points list must be provided.");

            RuleForEach(request => request.MedicinePointsList).NotNull().SetValidator(new UpdateMedicinePointsRequestValidator()).WithMessage("Valid medicine points must be provided.");
        }
    }
}