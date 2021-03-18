using Farmacio_Models.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Repositories.Contracts.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        T Create(T entity);
        IEnumerable<T> Read();
        T Read(Guid id);
        T Update(T entity);
        T Delete(Guid id);
    }
}
