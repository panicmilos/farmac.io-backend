using System;
using System.Collections.Generic;
using Farmacio_Models.Contracts;

namespace Farmacio_Services.Contracts
{
    public interface ICrudService<T> where T : IEntity
    {
        T Create(T entity);
        IEnumerable<T> Read();
        T Read(Guid id);
        T Update(T entity);
        T Delete(Guid id);
    }
}
