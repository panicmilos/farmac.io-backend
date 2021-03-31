namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreateSystemAdminRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreateSystemAdminUserRequest User { get; set; }
    }
}