namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class UpdateSupplierRequest
    {
        public UpdateAccountRequest Account { get; set; }
        public UpdateSupplierUserRequest User { get; set; }
    }
}