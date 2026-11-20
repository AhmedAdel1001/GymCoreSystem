using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Member> GetAllWithHealthRecord()
        {
            return _dbContext.Members
                .Include(m => m.HealthRecord)
                .AsNoTracking()
                .OrderByDescending(m => m.JoinDate)
                .ToList();
        }

        public Member? GetMemberWithDetails(int id)
        {
            return _dbContext.Members
                .Include(m => m.HealthRecord)
                .Include(m => m.Memberships)
                    .ThenInclude(ms => ms.Plan)
                .Include(m => m.Bookings)
                    .ThenInclude(b => b.Session)
                .FirstOrDefault(m => m.Id == id);
        }

        public Member? GetWithHealthRecord(int id)
        {
            return _dbContext.Members
                .Include(m => m.HealthRecord)
                .FirstOrDefault(m => m.Id == id);
        }

        public bool HasActiveBookings(int memberId)
        {
            return _dbContext.Bookings
                .Include(b => b.Session)
                .Any(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now);
        }

        public bool EmailExists(string email, int? excludeMemberId = null)
        {
            if (excludeMemberId.HasValue)
                return _dbContext.GymUsers.Any(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeMemberId.Value);
            return _dbContext.GymUsers.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool PhoneExists(string phone, int? excludeMemberId = null)
        {
            if (excludeMemberId.HasValue)
                return _dbContext.GymUsers.Any(u => u.Phone == phone && u.Id != excludeMemberId.Value);
            return _dbContext.GymUsers.Any(u => u.Phone == phone);
        }
    }
}

