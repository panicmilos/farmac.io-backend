namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdateSystemAdminRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdateSystemAdminUserRequest User { get; set; }
    }
}