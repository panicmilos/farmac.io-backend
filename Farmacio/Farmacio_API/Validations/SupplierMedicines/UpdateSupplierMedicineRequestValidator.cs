using Farmacio_API.Contracts.Requests.SupplierMedicines;
using FluentValidation;

namespace Farmacio_API.Validations.SupplierMedicines
{
    public class UpdateSupplierMedicineRequestValidator : AbstractValidator<UpdateSupplierMedicineRequest>
    {
        public UpdateSupplierMedicineRequestValidator()
        {
            RuleFor(request => request.Id).NotNull().WithMessage("Id must be provided.");

            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}