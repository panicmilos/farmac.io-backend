namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePatientRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreatePatientUserRequest User { get; set; }
    }
}