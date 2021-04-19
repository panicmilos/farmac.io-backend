using Farmacio_API.Contracts.Requests.Accounts;
using Swashbuckle.AspNetCore.Filters;

namespace Farmacio_API.Examples.Examples
{
    public class ChangePasswordRequestExample : IExamplesProvider<ChangePasswordRequest>
    {
        public ChangePasswordRequest GetExamples()
        {
            return new ChangePasswordRequest
            {
                CurrentPassword = "123eb@y321",
                NewPassword = "eb@y1234"
            };
        }
    }
}