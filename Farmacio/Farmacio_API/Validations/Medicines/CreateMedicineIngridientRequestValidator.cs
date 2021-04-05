using Farmacio_API.Contracts.Requests.Medicines;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class CreateMedicineIngridientRequestValidator : AbstractValidator<CreateMedicineIngridientRequest>
    {
        public CreateMedicineIngridientRequestValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty().WithMessage("Ingridient name must be provided.");

            RuleFor(request => request.MassInMilligrams).GreaterThan(0).WithMessage("Ingridient mass must be grater than 0.");
        }
    }
}