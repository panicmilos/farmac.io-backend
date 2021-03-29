namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreatePharmacyAdminRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreatePharmacyAdminUserRequest User { get; set; }
    }
}