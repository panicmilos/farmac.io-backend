using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class CrudService<T> : ICrudService<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;
        public CrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T Create(T entity)
        {
            return _repository.Create(entity);
        }

        public IEnumerable<T> Read()
        {
            return _repository.Read();
        }

        public T Read(Guid id)
        {
            return _repository.Read(id);
        }

        public virtual T Update(T entity)
        {
            var entityForUpdate = _repository.Update(entity);
            if (entityForUpdate == null)
                throw new MissingEntityException();
            return entityForUpdate;

        }

        public virtual T Delete(Guid id)
        {
            var entityForDeletion = _repository.Delete(id);
            if (entityForDeletion == null)
            {
                throw new MissingEntityException();
            }

            return entityForDeletion;
        }
    }
}
