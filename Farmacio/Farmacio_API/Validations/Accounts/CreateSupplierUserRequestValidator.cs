using Farmacio_API.Contracts.Requests.Accounts;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateSupplierUserRequestValidator : CreateUserRequestValidator<CreateSupplierUserRequest>
    {
        public CreateSupplierUserRequestValidator() :
            base()
        {
        }
    }
}