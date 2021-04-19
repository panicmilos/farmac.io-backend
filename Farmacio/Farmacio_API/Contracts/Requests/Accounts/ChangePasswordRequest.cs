namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}