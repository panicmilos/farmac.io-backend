using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(request => request.Username).NotNull().NotEmpty().WithMessage("Username must be provided.");
            RuleFor(request => request.Password).NotEmpty().Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("Password must have at least 8 characters, special character and a number.");
            RuleFor(request => request.Email).EmailAddress().WithMessage("Valid email must be provided");
        }
    }
}