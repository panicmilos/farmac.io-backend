namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdatePharmacyAdminRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdatePharmacyAdminUserRequest User { get; set; }
    }
}