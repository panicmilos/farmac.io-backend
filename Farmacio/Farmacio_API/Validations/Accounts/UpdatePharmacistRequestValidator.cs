using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacistRequestValidator : AbstractValidator<UpdatePharmacistRequest>
    {
        public UpdatePharmacistRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new UpdateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new UpdatePharmacistUserRequestValidator()).WithMessage("Valid Pharmacist must be provided.");
        }
    }
}