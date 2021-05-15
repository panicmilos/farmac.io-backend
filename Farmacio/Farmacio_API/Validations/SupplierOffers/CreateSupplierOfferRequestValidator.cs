using Farmacio_API.Contracts.Requests.SupplierOffers;
using FluentValidation;

namespace Farmacio_API.Validations.SupplierOffers
{
    public class CreateSupplierOfferRequestValidator : AbstractValidator<CreateSupplierOfferRequest>
    {
        public CreateSupplierOfferRequestValidator()
        {
            RuleFor(request => request.SupplierId).NotNull().WithMessage("Supplier Id must be provided.");

            RuleFor(request => request.PharmacyOrderId).NotNull().WithMessage("Pharmacy Order Id must be provided.");

            RuleFor(request => request.TotalPrice).GreaterThan(0).WithMessage("Total price must be greater than 0.");
        }
    }
}