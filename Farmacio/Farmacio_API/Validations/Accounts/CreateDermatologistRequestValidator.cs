using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateDermatologistRequestValidator : AbstractValidator<CreateDermatologistRequest>
    {
        public CreateDermatologistRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new CreateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new CreateDermatologistUserRequestValidator()).WithMessage("Valid dermatologist must be provided.");
        }
    }
}