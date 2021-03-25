using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacyAdminRequestValidator : AbstractValidator<UpdatePharmacyAdminRequest>
    {
        public UpdatePharmacyAdminRequestValidator()
        {
            RuleFor(request => request.Account).SetValidator(new UpdateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).SetValidator(new UpdatePharmacyAdminUserRequestValidator()).WithMessage("Valid PharmacyAdmin must be provided.");
        }
    }
}