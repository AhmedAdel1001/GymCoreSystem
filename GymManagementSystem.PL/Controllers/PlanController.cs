using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        #region Get All Plans
        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
        #endregion

        #region Plan Details
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan ID must be greater than 0.";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        #endregion

        #region Edit Plan
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan ID must be greater than 0.";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (it may be inactive or have active memberships).";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdatePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _planService.UpdatePlan(id, model);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Plan updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View(model);
        }
        #endregion

        #region Toggle Plan Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            var result = _planService.Activate(id);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Plan status updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

