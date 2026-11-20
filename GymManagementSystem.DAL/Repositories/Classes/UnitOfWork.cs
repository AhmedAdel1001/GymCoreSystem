using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public IPlanRepository PlanRepository { get; }
        public IMemberRepository MemberRepository { get; }
        public ISessionRepository SessionRepository { get; }

        public UnitOfWork(
            GymDbContext dbContext,
            IPlanRepository planRepository,
            IMemberRepository memberRepository,
            ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            PlanRepository = planRepository;
            MemberRepository = memberRepository;
            SessionRepository = sessionRepository;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity), _ => new GenericRepository<TEntity>(_dbContext));
        }

        public int SaveChanges() => _dbContext.SaveChanges();

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

