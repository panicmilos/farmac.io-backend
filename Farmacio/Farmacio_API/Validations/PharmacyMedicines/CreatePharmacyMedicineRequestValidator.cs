using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyMedicines
{
    public class CreatePharmacyMedicineRequestValidator : AbstractValidator<CreatePharmacyMedicineRequest>
    {
        public CreatePharmacyMedicineRequestValidator()
        {
            RuleFor(request => request.MedicineId).NotNull().NotEmpty().WithMessage("Medicine id must be provided.");
            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be grater than 0.");
        }
    }
}