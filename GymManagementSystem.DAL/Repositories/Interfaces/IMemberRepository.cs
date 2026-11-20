using GymManagementSystem.DAL.Entities;
using System.Collections.Generic;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        IEnumerable<Member> GetAllWithHealthRecord();
        Member? GetMemberWithDetails(int id);
        Member? GetWithHealthRecord(int id);
        bool HasActiveBookings(int memberId);
        bool EmailExists(string email, int? excludeMemberId = null);
        bool PhoneExists(string phone, int? excludeMemberId = null);
    }
}

