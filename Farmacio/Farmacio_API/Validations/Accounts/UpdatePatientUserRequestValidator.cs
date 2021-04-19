using Farmacio_API.Contracts.Requests.Accounts;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePatientUserRequestValidator : UpdateUserRequestValidator<UpdatePatientUserRequest>
    {
        public UpdatePatientUserRequestValidator() :
            base()
        {
        }
    }
}