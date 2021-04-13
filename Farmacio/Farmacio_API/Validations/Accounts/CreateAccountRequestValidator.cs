using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Validations.Extensions;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(request => request.Username).Username().WithMessage("Username must be alphanumerical word.");
            RuleFor(request => request.Password).Password().WithMessage("Password must have at least 8 characters, special character and a number.");
            RuleFor(request => request.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Valid email must be provided");
        }
    }
}