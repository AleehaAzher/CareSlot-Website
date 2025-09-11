using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Application.Services;
using InternshipFinalProject_Core.HelperClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InternshipProject.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var role = GetCurrentUserRole();

            if (role == "Doctor")
            {
                 return RedirectToAction("Status", "Doctor");
            }

            if (role == "Patient")
            {
                return RedirectToAction("profileStatus","Patient");
            }          
            if (role == "Admin")
                return RedirectToAction("Index");

            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult DoctorDashboard()
        {
            var userId = GetCurrentUserId();
            return View();
        }

        [Authorize(Roles = "Patient")]
        public IActionResult PatientDashboard()
        {
            var userId = GetCurrentUserId();
            var name = GetCurrentUserName();
            ViewBag.name = name;
            ViewBag.userid = userId;
            return View();
        }

        public IActionResult ManageDoctors()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CurrentRole()
        {
            var role = GetCurrentUserRole();
            return Json(new { role });
        }

        [HttpGet]
        public IActionResult CurrentName()
        {
            var name = GetCurrentUserName();
            return Json(new { name });
        }
        public IActionResult ForgotPasswordView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await userService.GeneratePasswordResetToken(model.Email);
            return Json(new { success = result });
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await userService.ResetPassword(model);
            if (result)
            {
                TempData["Message"] = "Password reset successfully!";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Invalid or expired token!";
            }
            return View(model);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult Support()
        {
            return View();
        }
    }
}
