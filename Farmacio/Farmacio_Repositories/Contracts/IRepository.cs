using Farmacio_Models.Contracts;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Repositories.Contracts
{
    public interface IRepository<T> where T : IEntity
    {
        T Create(T entity);

        IEnumerable<T> Create(IEnumerable<T> entities);

        IEnumerable<T> Read();

        IEnumerable<T> Read(Predicate<T> predicate);

        T Read(Guid id);

        IEnumerable<T> ReadPage(PageDTO pageDto);

        IEnumerable<T> ReadAllPagesTo(PageDTO pageDto);

        T Update(T entity);

        T Delete(Guid id);

        IEnumerable<T> Delete(IEnumerable<Guid> ids);

        IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector);

        IEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector);

        ITransaction OpenTransaction();
    }
}