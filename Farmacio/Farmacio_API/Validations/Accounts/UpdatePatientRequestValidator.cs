using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
    {
        public UpdatePatientRequestValidator()
        {
            RuleFor(request => request.Account).NotNull().SetValidator(new UpdateAccountRequestValidator()).WithMessage("Valid account must be provided.");

            RuleFor(request => request.User).NotNull().SetValidator(new UpdatePatientUserRequestValidator()).WithMessage("Valid patient must be provided.");
        }
    }
}