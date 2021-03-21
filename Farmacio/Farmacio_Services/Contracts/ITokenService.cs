using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface ITokenService
    {
        string GenerateFor(Account account);
    }
}
