using Farmacio_API.Contracts.Requests.Accounts;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateSystemAdminUserRequestValidator : CreateUserRequestValidator<CreateSystemAdminUserRequest>
    {
        public CreateSystemAdminUserRequestValidator() :
            base()
        {
        }
    }
}