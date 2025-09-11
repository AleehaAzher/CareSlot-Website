using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IAdminRepository
    {
        Task<AdminModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<UserCreationResult> updateAdminProfile(AdminDTO adminDtoObj);
    }
}
