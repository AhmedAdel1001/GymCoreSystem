using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Linq;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            var now = DateTime.Now;
            var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            var memberships = _unitOfWork.GetRepository<Membership>().GetAll();

            return new AnalyticsViewModel
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().Count(),
                ActiveMembers = memberships.Count(m => m.EndDate > now),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().Count(),
                UpcomingSessions = sessions.Count(s => s.StartDate > now),
                OngoingSessions = sessions.Count(s => s.StartDate <= now && s.EndDate >= now),
                CompletedSessions = sessions.Count(s => s.EndDate < now)
            };
        }
    }
}

