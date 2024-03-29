﻿using System;
using System.Collections.Generic;
using Farmacio_Models.Contracts;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface ICrudService<T> where T : IEntity
    {
        T Create(T entity);

        IEnumerable<T> Create(IEnumerable<T> entities);

        IEnumerable<T> Read();

        IEnumerable<T> Read(Predicate<T> predicate);

        T Read(Guid id);

        IEnumerable<T> ReadPage(PageDTO pageDto);

        IEnumerable<T> ReadAllPagesTo(PageDTO pageDto);

        T TryToRead(Guid id);

        T Update(T entity);

        T Delete(Guid id);

        IEnumerable<T> Delete(IEnumerable<Guid> ids);

        IEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector);

        IEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector);
    }
}