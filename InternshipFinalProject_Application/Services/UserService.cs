using InternshipFinalProject_Application.ViewModels;
using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using InternshipFinalProject_Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository userRepo;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        public async Task<UserCreationResult> CreateUser(UserModel userModelObj)
        {
            userModelObj.PasswordHash = HashPassword(userModelObj.PasswordHash);
            return await userRepo.Create(userModelObj);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public async Task<LoginSuccessData> LoginUser(LoginViewModel loginViewModelObj)
        {
            var email = loginViewModelObj.Email;
            var password = loginViewModelObj.PasswordHash;
            var user = await userRepo.GetUserByEmail(email, password);
            LoginSuccessData loginSuccessDataObj = new LoginSuccessData();

            if (user == null)
            {
                loginSuccessDataObj.Success = false;
                loginSuccessDataObj.Message = "Invalid User credentials.";
                loginSuccessDataObj.UserId = 0;
                loginSuccessDataObj.FullName = null;
                loginSuccessDataObj.Email = null;
                loginSuccessDataObj.Role = null;
                return loginSuccessDataObj;
            }
            loginSuccessDataObj.Success = true;
            loginSuccessDataObj.Message = "Login successful";
            loginSuccessDataObj.UserId = user.UserId;
            loginSuccessDataObj.FullName = user.FullName;
            loginSuccessDataObj.Email = user.Email;
            loginSuccessDataObj.Role = user.Role;

            return loginSuccessDataObj;
        }
        public async Task<UserModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            return await userRepo.GetUserByIdOrEmail(id, email);
        }
        public async Task<List<UserModel>> ReadUsers()
        {
            return await userRepo.GetAllUsers();
        }
        public async Task<UserCreationResult> Update(UserModel userModelObj)
        {
            return await userRepo.Update(userModelObj);
        }
        public async Task<bool> GeneratePasswordResetToken(string email)
        {
            var user = await userRepo.GetUserByIdOrEmail(null, email);
            if (user == null)
            {
                return false;
            }
            user.ResetToken = Guid.NewGuid().ToString();
            user.TokenExpiry = DateTime.UtcNow.AddHours(1);

            await userRepo.Update(user);
            var resetLink = $"https://localhost:7193/Home/ResetPassword?token={user.ResetToken}";
            SendResetEmail(user.Email, resetLink);

            return true;
        }

        private void SendResetEmail(string email, string resetLink)
        {
            var fromAddress = new System.Net.Mail.MailAddress("aleehaazher8@gmail.com", "CareSlot");
            var toAddress = new System.Net.Mail.MailAddress(email);
            const string fromPassword = "rkng ttkn lpoz qdgs";
            const string subject = "Password Reset";
            string body = $"Click here to reset your password: {resetLink}";

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public async Task<UserModel?> GetUserByToken(string token)
        {
            var users= await userRepo.GetAllUsers();
            var user = users.FirstOrDefault(u => u.ResetToken == token && u.TokenExpiry > DateTime.UtcNow);
            return user;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await GetUserByToken(model.Token);
            if (user == null || user.Email != model.Email)
            {
                return false;
            }
            user.PasswordHash = HashPassword(model.NewPassword);
            user.ResetToken = null;
            user.TokenExpiry = null;

            var response=await userRepo.Update(user);
            if(response==null)
            {
                return false;
            }
            return true;
        }

    }
}
