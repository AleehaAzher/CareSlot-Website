using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternshipProject.Controllers
{
    public class BaseController : Controller
    {
        protected string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        protected string GetCurrentUserName()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }

        protected string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
