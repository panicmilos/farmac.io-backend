using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IAuthenticationService
    {
        AuthenticatedUserDTO Authenticate(string username, string password);
    }
}