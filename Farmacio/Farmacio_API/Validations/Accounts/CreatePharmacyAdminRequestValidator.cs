using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreatePharmacyAdminRequestValidator : AbstractValidator<CreatePharmacyAdminRequest>
    {
        public CreatePharmacyAdminRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new CreateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new CreatePharmacyAdminUserRequestValidator()).WithMessage("Valid user must be provided.");
        }
    }
}