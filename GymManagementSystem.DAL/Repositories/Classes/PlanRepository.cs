using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Linq;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {
        }

        public bool HasActiveMemberships(int planId)
        {
            return _dbContext.Memberships.Any(m => m.PlanId == planId && m.EndDate > DateTime.Now);
        }
    }
}

