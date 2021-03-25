using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IEmailVerificationService
    {
        void SendTo(Account account);
    }
}