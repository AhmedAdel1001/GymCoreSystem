using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System.Collections.Generic;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        MemberViewModel? GetMemberDetails(int id);
        HealthRecordViewModel? GetMemberHealthRecord(int id);
        Result CreateMember(CreateMemberViewModel model);
        MemberToUpdateViewModel? GetMemberToUpdate(int id);
        Result UpdateMemberDetails(int id, MemberToUpdateViewModel model);
        Result RemoveMember(int id);
    }
}

