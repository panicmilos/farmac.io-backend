using System;
using System.Collections.Generic;
using Farmacio_Models.Contracts;

namespace Farmacio_Repositories.Contracts
{
    public interface IRepository<T> where T : IEntity
    {
        T Create(T entity);
        IEnumerable<T> Read();
        T Read(Guid id);
        T Update(T entity);
        T Delete(Guid id);
        void SaveChanges();
    }
}
