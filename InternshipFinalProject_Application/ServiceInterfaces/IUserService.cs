using InternshipFinalProject_Application.DTOs;
using InternshipFinalProject_Application.ViewModels;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserCreationResult> CreateUser(UserModel userModelObj);
        Task<LoginSuccessData> LoginUser(LoginViewModel loginViewModelObj);
        Task<UserModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<List<UserModel>> ReadUsers();
        Task<bool> GeneratePasswordResetToken(string email);
        Task<UserModel?> GetUserByToken(string token);
        Task<UserCreationResult> Update(UserModel userModelObj);
        Task<bool> ResetPassword(ResetPasswordViewModel model);
    }
}
