using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System.Collections.Generic;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int sessionId);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        Result CreateSession(CreateSessionViewModel createSession);
        Result UpdateSession(int id, UpdateSessionViewModel updateSession);
        Result RemoveSession(int sessionId);
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();
    }
}

