namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreateSupplierRequest
    {
        public CreateAccountRequest Account { get; set; }
        public CreateSupplierUserRequest User { get; set; }
    }
}