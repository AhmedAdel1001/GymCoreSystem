using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly GymDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
                return _dbSet.AsNoTracking().Where(predicate).ToList();
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual TEntity? GetById(int id) => _dbSet.Find(id);

        public virtual void Add(TEntity entity) => _dbSet.Add(entity);

        public virtual void Update(TEntity entity) => _dbSet.Update(entity);

        public virtual void Delete(TEntity entity) => _dbSet.Remove(entity);

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate) => _dbSet.Any(predicate);

        public virtual int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
                return _dbSet.Count(predicate);
            return _dbSet.Count();
        }
    }
}

