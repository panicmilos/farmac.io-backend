using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Repositories.Implementation
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(DatabaseContext context) :
            base(context)
        {
        }

        public IEnumerable<Account> ReadPageOf(Role role, PageDTO pageDto)
        {
            var numberOfEntitiesToSkip = (pageDto.Number - 1) * pageDto.Size;
            return _entities.Where(e => e.Active && e.Role == role)
                .Skip(numberOfEntitiesToSkip)
                .Take(pageDto.Size);
        }
    }
}