using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .ToList();
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefault(s => s.Id == sessionId);
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.Bookings.Count(b => b.SessionId == sessionId);
        }

        public bool HasTrainerOverlap(int trainerId, DateTime startDate, DateTime endDate, int? excludeSessionId = null)
        {
            var query = _dbContext.Sessions.Where(s => s.TrainerId == trainerId);
            if (excludeSessionId.HasValue)
                query = query.Where(s => s.Id != excludeSessionId.Value);

            // Overlap condition: s.StartDate < endDate && s.EndDate > startDate
            return query.Any(s => s.StartDate < endDate && s.EndDate > startDate);
        }
    }
}

