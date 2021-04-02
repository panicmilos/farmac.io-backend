using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using Farmacio_Repositories.Contracts;

namespace Farmacio_Services.Implementation
{
    public class CrudService<T> : ICrudService<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;

        public CrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T Create(T entity)
        {
            return _repository.Create(entity);
        }

        public virtual IEnumerable<T> Read()
        {
            return _repository.Read();
        }

        public virtual T Read(Guid id)
        {
            return _repository.Read(id);
        }

        public virtual T TryToRead(Guid id)
        {
            var existingEntity = Read(id);
            if(existingEntity == null)
                throw new MissingEntityException($"{nameof(T)} not found.");
            return existingEntity;
        }

        public virtual T Update(T entity)
        {
            var entityForUpdate = _repository.Update(entity);
            if (entityForUpdate == null)
            {
                throw new MissingEntityException();
            }

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