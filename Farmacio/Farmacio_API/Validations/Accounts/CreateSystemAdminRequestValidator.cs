using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateSystemAdminRequestValidator : AbstractValidator<CreateSystemAdminRequest>
    {
        public CreateSystemAdminRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new CreateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new CreateSystemAdminUserRequestValidator()).WithMessage("Valid system admin must be provided.");
        }
    }
}