using Farmacio_API.Contracts.Requests.Medicines;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class CreateMedicineTypeRequestValidator : AbstractValidator<CreateMedicineTypeRequest>
    {
        public CreateMedicineTypeRequestValidator()
        {
            RuleFor(request => request.TypeName).NotNull().NotEmpty().WithMessage("Type must be provided.");
        }
    }
}