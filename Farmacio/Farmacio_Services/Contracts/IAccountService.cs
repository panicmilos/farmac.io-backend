using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IAccountService : ICrudService<Account>
    {
        Account ChangePasswordFor(Guid id, string currentPassword, string newPassword);

        Account ReadByEmail(string email);

        Account Verify(Guid id);

        IEnumerable<Account> SearchByName(string name);

        Account ReadByUserId(Guid userId);

        IEnumerable<Account> ReadPageOf(Role role, PageDTO page);
    }
}