using Farmacio_Models.Domain;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IAccountService : ICrudService<Account>
    {
        Account ReadByEmail(string email);

        Account Verify(Guid id);
        IEnumerable<Account> SearchByName(string name);
        Account ReadByUserId(Guid userId);
    }
}