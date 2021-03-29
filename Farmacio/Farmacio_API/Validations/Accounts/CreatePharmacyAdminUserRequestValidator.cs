using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class CreatePharmacyAdminUserRequestValidator : CreateUserRequestValidator<CreatePharmacyAdminUserRequest>
    {
        public CreatePharmacyAdminUserRequestValidator()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");
        }
    }
}