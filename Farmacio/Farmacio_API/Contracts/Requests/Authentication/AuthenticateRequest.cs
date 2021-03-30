namespace Farmacio_API.Contracts.Requests.Authentication
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}