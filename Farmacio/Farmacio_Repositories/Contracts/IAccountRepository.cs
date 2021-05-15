using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System.Collections.Generic;

namespace Farmacio_Repositories.Contracts
{
    public interface IAccountRepository : IRepository<Account>
    {
        IEnumerable<Account> ReadPageOf(Role role, PageDTO page);
    }
}