using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace InternshipFinalProject.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }
       
    }
}
