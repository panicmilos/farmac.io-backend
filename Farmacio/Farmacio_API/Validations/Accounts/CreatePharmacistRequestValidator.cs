using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreatePharmacistRequestValidator : AbstractValidator<CreatePharmacistRequest>
    {
        public CreatePharmacistRequestValidator()
        {
            RuleFor(request => request.Account).SetValidator(new CreateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).SetValidator(new CreatePharmacistUserRequestValidator())
                .WithMessage("Valid Pharmacist must be provided.");
        }
    }
}