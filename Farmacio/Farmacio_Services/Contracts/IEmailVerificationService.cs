using System;

namespace Farmacio_Services.Contracts
{
    public interface IEmailVerificationService
    {
        void SendTo(string email);

        void Verify(Guid accountId);
    }
}