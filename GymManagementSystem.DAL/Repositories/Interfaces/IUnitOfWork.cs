using GymManagementSystem.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        IPlanRepository PlanRepository { get; }
        IMemberRepository MemberRepository { get; }
        ISessionRepository SessionRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}

