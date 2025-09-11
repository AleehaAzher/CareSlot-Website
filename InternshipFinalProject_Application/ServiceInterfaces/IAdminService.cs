using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.ServiceInterfaces
{
    public interface IAdminService
    {
        Task<AdminModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<UserCreationResult> updateAdmin(AdminDTO adminDtoObj);
    }
}
