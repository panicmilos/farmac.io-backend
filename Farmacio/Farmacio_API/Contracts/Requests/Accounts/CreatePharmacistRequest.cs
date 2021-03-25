namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePharmacistRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreatePharmacistUserRequest User { get; set; }
    }
}