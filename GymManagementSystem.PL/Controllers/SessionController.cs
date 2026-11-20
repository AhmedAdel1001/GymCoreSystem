using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }
        #endregion

        #region Create Session
        public IActionResult Create()
        {
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View(new CreateSessionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(model);
            }

            var result = _sessionService.CreateSession(model);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.ErrorMessage);
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View(model);
        }
        #endregion

        #region Session Details
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }
        #endregion

        #region Edit Session
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session cannot be updated (only upcoming sessions can be modified).";
                return RedirectToAction(nameof(Index));
            }

            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(model);
            }

            var result = _sessionService.UpdateSession(id, model);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View(model);
        }
        #endregion

        #region Delete Session
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper Methods
        private void LoadCategoriesDropDown()
        {
            var categories = _sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        private void LoadTrainersDropDown()
        {
            var trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion
    }
}

