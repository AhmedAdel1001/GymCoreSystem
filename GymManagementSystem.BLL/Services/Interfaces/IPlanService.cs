using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using System.Collections.Generic;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        Result UpdatePlan(int id, UpdatePlanViewModel model);
        Result Activate(int planId);
    }
}

