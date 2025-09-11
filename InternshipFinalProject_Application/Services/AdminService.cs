using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.Services
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository adminRepo;

        public AdminService(IAdminRepository adminRepo) 
        {
            this.adminRepo = adminRepo;
        }
        public async Task<AdminModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            return await adminRepo.GetUserByIdOrEmail(id, email);
        }
        public async Task<UserCreationResult> updateAdmin(AdminDTO adminDtoObj)
        {
            if (!string.IsNullOrEmpty(adminDtoObj.password))
            {
                adminDtoObj.password = HashPassword(adminDtoObj.password);
            }

            return await adminRepo.updateAdminProfile(adminDtoObj);
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
    }
}
