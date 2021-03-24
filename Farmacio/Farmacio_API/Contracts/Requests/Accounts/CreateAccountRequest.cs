namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreateAccountRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}