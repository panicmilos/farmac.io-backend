using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
    {
        public CreatePatientRequestValidator()
        {
            RuleFor(request => request.Account).SetValidator(new CreateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).SetValidator(new CreateUserRequestValidator<CreatePatientUserRequest>()).WithMessage("Valid user must be provided.");
        }
    }
}