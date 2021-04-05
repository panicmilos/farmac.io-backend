using Farmacio_API.Contracts.Requests.Medicines;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class CreateMedicineReplacementRequestValidator : AbstractValidator<CreateMedicineReplacementRequest>
    {
        public CreateMedicineReplacementRequestValidator()
        {
            RuleFor(request => request.ReplacementMedicineId).NotNull().WithMessage("Replacement medicine id must be provided.");
        }
    }
}