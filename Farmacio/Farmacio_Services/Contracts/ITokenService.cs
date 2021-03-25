using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface ITokenService
    {
        string GenerateAuthTokenFor(Account account);

        string GenerateEmailTokenFor(Account account);
    }
}