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

        public virtual IEnumerable<T> Read()
        {
            var allEntities = _entities.Where(entity => entity.Active);

            return allEntities;
        }

        public virtual T Read(Guid id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id && e.Active);

            return entity;
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
    }
}