using Farmacio_API.Contracts.Requests.Medicines;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class UpdateMedicineReplacementRequestValidator : AbstractValidator<UpdateMedicineReplacementRequest>
    {
        public UpdateMedicineReplacementRequestValidator()
        {
            RuleFor(request => request.ReplacementMedicineId).NotNull().WithMessage("Replacement medicine id must be provided.");
        }
    }
}