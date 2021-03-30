namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdateDermatologistRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdateDermatologistUserRequest User { get; set; }
    }
}