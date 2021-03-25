using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface IAccountService : ICrudService<Account>
    {
        Account ReadByEmail(string email);

        Account Verify(Guid id);
    }
}