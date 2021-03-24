using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(request => request.Username).NotNull().NotEmpty().WithMessage("Username must be provided.");
            RuleFor(request => request.Password).NotEmpty().MinimumLength(8).WithMessage("Password must have at least 8 characters.");
            RuleFor(request => request.Email).EmailAddress().WithMessage("Valid email must be provided");
        }
    }
}