using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdateDermatologistRequestValidator : AbstractValidator<UpdateDermatologistRequest>
    {
        public UpdateDermatologistRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new UpdateAccountRequestValidator()).WithMessage("Valid account must be provided.");
            RuleFor(request => request.User).NotNull().SetValidator(new UpdateDermatologistUserRequestValidator()).WithMessage("Valid dermatologist must be provided.");
        }
    }
}