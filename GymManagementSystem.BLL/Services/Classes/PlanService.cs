using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.PlanRepository.GetAll();
            if (!plans.Any()) return Enumerable.Empty<PlanViewModel>();
            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var plan = _unitOfWork.PlanRepository.GetById(planId);
            return plan == null ? null : _mapper.Map<PlanViewModel>(plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.PlanRepository.GetById(planId);
            if (plan == null || !plan.IsActive || _unitOfWork.PlanRepository.HasActiveMemberships(planId))
                return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public Result UpdatePlan(int id, UpdatePlanViewModel model)
        {
            try
            {
                var plan = _unitOfWork.PlanRepository.GetById(id);
                if (plan == null)
                    return Result.Fail("Plan not found.");

                if (_unitOfWork.PlanRepository.HasActiveMemberships(id))
                    return Result.Fail("Cannot update plan with active memberships.");

                if (model.DurationDays < 1 || model.DurationDays > 365)
                    return Result.Fail("Plan duration must be between 1 and 365 days.");

                _mapper.Map(model, plan);
                plan.UpdatedAt = DateTime.Now;

                _unitOfWork.PlanRepository.Update(plan);
                var saved = _unitOfWork.SaveChanges() > 0;
                return saved ? Result.Ok() : Result.Fail("Failed to save changes to the plan.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while updating the plan: {ex.Message}");
            }
        }

        public Result Activate(int planId)
        {
            try
            {
                var plan = _unitOfWork.PlanRepository.GetById(planId);
                if (plan == null)
                    return Result.Fail("Plan not found.");

                if (plan.IsActive && _unitOfWork.PlanRepository.HasActiveMemberships(planId))
                    return Result.Fail("Cannot deactivate plan with active memberships.");

                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.Now;

                _unitOfWork.PlanRepository.Update(plan);
                var saved = _unitOfWork.SaveChanges() > 0;
                return saved ? Result.Ok() : Result.Fail("Failed to update plan status.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while toggling plan status: {ex.Message}");
            }
        }
    }
}

