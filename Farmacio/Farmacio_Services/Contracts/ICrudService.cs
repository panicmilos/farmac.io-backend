using Farmacio_Models.Contracts.Entities;
using System;
using System.Collections.Generic;

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
