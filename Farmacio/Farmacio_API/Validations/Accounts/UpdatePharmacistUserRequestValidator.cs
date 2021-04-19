using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacistUserRequestValidator : UpdateUserRequestValidator<UpdatePharmacistUserRequest>
    {
        public UpdatePharmacistUserRequestValidator() : base()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");
            RuleFor(request => request.WorkTime).NotNull().WithMessage("Work time must be provided.");
        }
    }
}