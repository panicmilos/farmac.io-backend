using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface ITokenProvider
    {
        string GenerateAuthTokenFor(Account account);

        string GenerateEmailTokenFor(Account account);
    }
}