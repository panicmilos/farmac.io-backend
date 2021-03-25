namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePharmacistRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdatePharmacistUserRequest User { get; set; }
    }
}