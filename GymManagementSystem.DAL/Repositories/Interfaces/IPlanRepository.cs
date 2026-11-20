using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        bool HasActiveMemberships(int planId);
    }
}

