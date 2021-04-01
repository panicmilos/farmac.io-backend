using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using FluentValidation;

namespace Farmacio_API.Validations.Medicines
{
    public class CreateMedicineRequestValidator : AbstractValidator<CreateMedicineRequest>
    {
        public CreateMedicineRequestValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty().WithMessage("Medicine name must be provided.");

            RuleFor(request => request.UniqueId).NotNull().NotEmpty().Matches("^[a-zA-Z0-9]{10}$").Length(10).WithMessage("Code must be 10 alphanumerical characters word.");

            RuleFor(request => request.Form).Must(form => form >= MedicineForm.Powder && form <= MedicineForm.Other).WithMessage("Medicine form is not valid.");

            RuleFor(request => request.Type).NotNull().SetValidator(new CreateMedicineTypeRequestValidator()).WithMessage("Valid medicine type must be provided.");

            RuleFor(request => request.Manufacturer).NotNull().NotEmpty().WithMessage("Manufecturer must be provided.");

            RuleFor(request => request.Contraindications).NotNull().NotEmpty().WithMessage("Contraindications must be provided.");

            RuleFor(request => request.AdditionalInfo).NotNull().NotEmpty().WithMessage("Additional info must be provided.");

            RuleFor(request => request.RecommendedDose).NotNull().NotEmpty().WithMessage("RecommendedDose must be provided.");

            RuleForEach(request => request.MedicineIngredients).NotNull().SetValidator(new CreateMedicineIngridientRequestValidator()).WithMessage("Valid medicine ingridient must be provided.");

            RuleForEach(request => request.Replacements).NotNull().SetValidator(new CreateMedicineReplacementRequestValidator()).WithMessage("Valid medicine replacement must be provided.");
        }
    }
}