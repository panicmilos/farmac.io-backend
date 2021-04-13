using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Validations.Extensions;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(request => request.NewPassword).Password().WithMessage("Password must have at least 8 characters, special character and a number.");
        }
    }
}