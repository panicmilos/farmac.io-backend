using Farmacio_API.Contracts.Requests.Accounts;

namespace Farmacio_API.Validations.Accounts
{
    public class CreateDermatologistUserRequestValidator : CreateUserRequestValidator<CreateDermatologistUserRequest>
    {
        public CreateDermatologistUserRequestValidator() :
            base()
        {
        }
    }
}