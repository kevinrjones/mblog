﻿using System;
using System.Linq;

namespace Repository
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> Entities { get; }
        T New();
        void Attach(T entity);
        void Create(T entity);
        void Delete(T entity);
        //void Save();
    }
}