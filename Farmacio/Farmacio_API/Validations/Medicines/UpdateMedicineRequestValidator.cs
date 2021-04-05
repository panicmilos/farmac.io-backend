using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using FluentValidation;
using System.Linq;

namespace Farmacio_API.Validations.Medicines
{
    public class UpdateMedicineRequestValidator : AbstractValidator<UpdateMedicineRequest>
    {
        public UpdateMedicineRequestValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty().WithMessage("Id must be provided.");

            RuleFor(request => request.Name).NotNull().NotEmpty().WithMessage("Medicine name must be provided.");

            RuleFor(request => request.Form).Must(form => form >= MedicineForm.Powder && form <= MedicineForm.Other).WithMessage("Medicine form is not valid.");

            RuleFor(request => request.Type).NotNull().SetValidator(new UpdateMedicineTypeRequestValidator()).WithMessage("Valid medicine type must be provided.");

            RuleFor(request => request.Manufacturer).NotNull().NotEmpty().WithMessage("Manufecturer must be provided.");

            RuleFor(request => request.Contraindications).NotNull().NotEmpty().WithMessage("Contraindications must be provided.");

            RuleFor(request => request.AdditionalInfo).NotNull().NotEmpty().WithMessage("Additional info must be provided.");

            RuleFor(request => request.RecommendedDose).NotNull().NotEmpty().WithMessage("RecommendedDose must be provided.");

            RuleForEach(request => request.MedicineIngredients).NotNull().SetValidator(new UpdateMedicineIngredientRequestValidator()).WithMessage("Valid medicine ingridient must be provided.");
            RuleFor(request => request.MedicineIngredients).Must(ings => ings.All(ing => ings.Count(ing2 => ing2.Name == ing.Name) == 1)).WithMessage("Ingredient names must be unique.");

            RuleForEach(request => request.Replacements).NotNull().SetValidator(new UpdateMedicineReplacementRequestValidator()).WithMessage("Valid medicine replacement must be provided.");
        }
    }
}