using Farmacio_Models.Contracts;
using System;
using System.Collections.Generic;
using Farmacio_Models.DTO;

namespace Farmacio_Repositories.Contracts
{
    public interface IRepository<T> where T : IEntity
    {
        T Create(T entity);

        IEnumerable<T> Create(IEnumerable<T> entities);

        IEnumerable<T> Read();
        IEnumerable<T> ReadPage(PageDTO pageDto);

        IEnumerable<T> Read(Predicate<T> predicate);

        T Read(Guid id);

        T Update(T entity);

        T Delete(Guid id);

        IEnumerable<T> Delete(IEnumerable<Guid> ids);

        IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector);

        IEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector);
    }
}