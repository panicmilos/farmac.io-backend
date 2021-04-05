using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual T Create(T entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public virtual IEnumerable<T> Create(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
            _context.SaveChanges();

            return entities;
        }

        public virtual IEnumerable<T> Read()
        {
            return _entities.Where(entity => entity.Active);
        }

        public IEnumerable<T> Read(Predicate<T> predicate)
        {
            return _entities.Where(entity => entity.Active && predicate(entity));
        }

        public virtual T Read(Guid id)
        {
            return _entities.FirstOrDefault(e => e.Id == id && e.Active);
        }

        public virtual T Update(T entity)
        {
            var entityForUpdate = Read(entity.Id);
            if (entityForUpdate != null)
            {
                _context.Entry(entityForUpdate).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }

            return entityForUpdate;
        }

        public virtual T Delete(Guid id)
        {
            var entityForDeletion = Read(id);
            if (entityForDeletion != null)
            {
                entityForDeletion.Active = false;
                Update(entityForDeletion);
            }

            return entityForDeletion;
        }

        public virtual IEnumerable<T> Delete(IEnumerable<Guid> ids)
        {
            var entities = _entities.Where(entity => entity.Active && ids.Contains(entity.Id)).ToList();
            foreach (var entity in entities)
            {
                entity.Active = false;
            }
            _entities.UpdateRange(entities);
            _context.SaveChanges();

            return entities;
        }

        public virtual IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            return _entities.OrderBy(keySelector);
        }

        public virtual IEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            return _entities.OrderByDescending(keySelector);
        }
    }
}