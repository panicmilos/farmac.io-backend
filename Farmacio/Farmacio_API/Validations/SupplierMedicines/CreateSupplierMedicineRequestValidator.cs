using Farmacio_API.Contracts.Requests.SupplierMedicines;
using FluentValidation;

namespace Farmacio_API.Validations.SupplierMedicines
{
    public class CreateSupplierMedicineRequestValidator : AbstractValidator<CreateSupplierMedicineRequest>
    {
        public CreateSupplierMedicineRequestValidator()
        {
            RuleFor(request => request.SupplierId).NotNull().WithMessage("SupplierId must be provided.");

            RuleFor(request => request.MedicineId).NotNull().WithMessage("MedicineId must be provided.");

            RuleFor(request => request.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be positive number.");
        }
    }
}