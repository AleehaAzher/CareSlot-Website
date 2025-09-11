using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<UserCreationResult> Create(UserModel userModelObj);
        Task<UserModel?> GetUserByEmail(string email, string password);
        Task<UserModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<List<UserModel>> GetAllUsers();
        Task<UserCreationResult> Update(UserModel userModelObj);

    }
}
