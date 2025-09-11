
using InternshipFinalProject_Application.ViewModels;
using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace InternshipProject.Controllers
{
    public class UserController : BaseController
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //[HttpGet]
        //public async Task<IActionResult> UserPage() 
        //{
        //    var currentUserId = GetCurrentUserId();
        //    List<UserModel> list = new List<UserModel>();
        //    HttpResponseMessage response = await client.GetAsync("GetAll");
        //    if (response.IsSuccessStatusCode) 
        //    { 
        //        string message = await response.Content.ReadAsStringAsync();
        //        var userlist = JsonConvert.DeserializeObject<List<UserModel>>(message);
        //        if (userlist != null)
        //        {
        //            list = userlist;
        //        } 
        //    } 
        //    return View(list);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await userService.CreateUser(user);

            if (response!=null)
            {
                
                return Json(new
                {
                    success = true,
                    message = "User created successfully",
                    user = response
                });
            }
            return Json(new { success = false, message = "Failed to create user" });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModelObj)
        {
            var result = await userService.LoginUser(loginViewModelObj);
            if (result!=null)
            {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                        new Claim(ClaimTypes.Name, result.FullName),
                        new Claim(ClaimTypes.Email, result.Email),
                        new Claim(ClaimTypes.Role, result.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return Json(new { success = true, message = "Login successful" });
                
            }
            return Json(new { success = false, message = result.Message });
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    HttpResponseMessage response = await client.GetAsync($"GetById?id={id}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = await response.Content.ReadAsStringAsync();
        //        var user = JsonConvert.DeserializeObject<UserModel>(result);

        //        return Json(new { success = true, data = user });
        //    }
        //    return Json(new { success = false, message = "User not found" });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit([FromBody] UserModel user)
        //{
        //    string data = JsonConvert.SerializeObject(user);
        //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PutAsync("Edit", content); 
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = await response.Content.ReadAsStringAsync();
        //        var apiResponse = JsonConvert.DeserializeObject<dynamic>(result);
        //        return Json(new
        //        {
        //            success = true,
        //            message = apiResponse.message 
        //        });
        //    }
        //    return Json(new { success = false, message = "Failed to update user" });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    HttpResponseMessage response = await client.DeleteAsync($"Delete/{id}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return Json(new { success = true, message = "User deleted successfully" });
        //    }
        //    return Json(new { success = false, message = "Failed to delete user" });
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //public async Task<IActionResult> ManageDoctors()
        //{
        //    return View();
        //}
    }
}
