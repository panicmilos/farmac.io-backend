using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreatePharmacistUserRequestValidator : CreateUserRequestValidator<CreatePharmacistUserRequest>
    {
        public CreatePharmacistUserRequestValidator() : base()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("Pharmacy Id can't be null.");
        }
    }
}