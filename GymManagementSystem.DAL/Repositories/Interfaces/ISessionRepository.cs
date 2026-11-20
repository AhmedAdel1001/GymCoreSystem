using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        Session? GetSessionWithTrainerAndCategory(int sessionId);
        int GetCountOfBookedSlots(int sessionId);
        bool HasTrainerOverlap(int trainerId, DateTime startDate, DateTime endDate, int? excludeSessionId = null);
    }
}

