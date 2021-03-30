namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreateDermatologistRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreateDermatologistUserRequest User { get; set; }
    }
}