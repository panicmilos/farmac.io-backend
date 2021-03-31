using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdateSystemAdminRequestValidator : AbstractValidator<UpdateSystemAdminRequest>
    {
        public UpdateSystemAdminRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new UpdateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new UpdateSystemAdminUserRequestValidator()).WithMessage("Valid system admin must be provided.");
        }
    }
}