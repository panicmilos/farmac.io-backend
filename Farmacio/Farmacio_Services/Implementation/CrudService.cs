using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using Farmacio_Models.DTO;
using System.Linq;

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

        public virtual IEnumerable<T> Create(IEnumerable<T> entities)
        {
            return _repository.Create(entities);
        }

        public virtual IEnumerable<T> Read()
        {
            return _repository.Read();
        }

        public IEnumerable<T> ReadPage(PageDTO pageDto)
        {
            return _repository.ReadPage(pageDto);
        }

        public IEnumerable<T> ReadAllPagesTo(PageDTO pageDto)
        {
            return _repository.ReadAllPagesTo(pageDto).ToList();
        }

        public virtual IEnumerable<T> Read(Predicate<T> predicate)
        {
            return _repository.Read(predicate);
        }

        public virtual T Read(Guid id)
        {
            return _repository.Read(id);
        }

        public virtual T TryToRead(Guid id)
        {
            var existingEntity = Read(id);
            if (existingEntity == null)
                throw new MissingEntityException($"{typeof(T).Name} not found.");
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

        public virtual IEnumerable<T> Delete(IEnumerable<Guid> ids)
        {
            return _repository.Delete(ids);
        }

        public virtual IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            return _repository.OrderBy(keySelector);
        }

        public virtual IEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            return _repository.OrderByDescending(keySelector);
        }
    }
}