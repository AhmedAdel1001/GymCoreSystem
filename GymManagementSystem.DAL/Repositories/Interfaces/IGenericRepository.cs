using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
        TEntity? GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>>? predicate = null);
    }
}

