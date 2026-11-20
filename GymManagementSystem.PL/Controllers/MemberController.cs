using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Members
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion

        #region Create Member
        public IActionResult Create()
        {
            return View(new CreateMemberViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Please verify all required fields.");
                return View(model);
            }

            var result = _memberService.CreateMember(model);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMember(CreateMemberViewModel model)
        {
            return Create(model);
        }
        #endregion

        #region Member Details
        public IActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public IActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var healthRecord = _memberService.GetMemberHealthRecord(id);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }
        #endregion

        #region Edit Member
        public IActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _memberService.UpdateMemberDetails(id, model);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View(model);
        }
        #endregion

        #region Delete Member
        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _memberService.RemoveMember(id);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member deleted successfully!";
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

