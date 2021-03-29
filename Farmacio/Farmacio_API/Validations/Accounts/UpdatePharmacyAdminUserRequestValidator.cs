using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacyAdminUserRequestValidator : UpdateUserRequestValidator<UpdatePharmacyAdminUserRequest>
    {
        public UpdatePharmacyAdminUserRequestValidator() :
            base()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");
        }
    }
}