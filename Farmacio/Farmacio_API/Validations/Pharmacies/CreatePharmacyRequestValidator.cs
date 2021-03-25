using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_API.Validations.Addresses;
using FluentValidation;

namespace Farmacio_API.Validations.Pharmacies
{
    public class CreatePharmacyRequestValidator : AbstractValidator<CreatePharmacyRequest>
    {
        public CreatePharmacyRequestValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty().WithMessage("Name must be provided.");
            RuleFor(request => request.Description).NotNull().NotEmpty().WithMessage("Description must be provided.");
            RuleFor(request => request.Address).SetValidator(new CreateAddressRequestValidator()).WithMessage("Valid address must be provided");
        }
    }
}