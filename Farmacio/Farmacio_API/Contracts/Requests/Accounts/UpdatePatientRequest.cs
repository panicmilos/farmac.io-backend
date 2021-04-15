namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePatientRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdatePatientUserRequest User { get; set; }
    }
}