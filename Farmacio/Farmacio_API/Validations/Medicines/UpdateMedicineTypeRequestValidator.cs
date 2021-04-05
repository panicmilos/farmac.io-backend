using Farmacio_API.Contracts.Requests.Medicines;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class UpdateMedicineTypeRequestValidator : AbstractValidator<UpdateMedicineTypeRequest>
    {
        public UpdateMedicineTypeRequestValidator()
        {
            RuleFor(request => request.TypeName).NotNull().NotEmpty().WithMessage("Type must be provided.");
        }
    }
}