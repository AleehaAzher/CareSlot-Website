using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using InternshipFinalProject_Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Infrastructure.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public AdminRepository(DataAccessClass dataAccessClassObj)
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<AdminModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            if (id == null && email != null)
            {
                return await dataAccessClassObj.AdminTable
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(x => x.User.Email == email);

            }
            else if (id != null && email == null)
            {
                var data = await dataAccessClassObj.AdminTable
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == id);
                return data;
            }
            else
            {
                return null;
            }
        }
        public async Task<UserCreationResult> updateAdminProfile(AdminDTO adminDtoObj)
        {
            var result = new UserCreationResult();

            try
            {
                var admin = await dataAccessClassObj.AdminTable
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.UserId == adminDtoObj.id);

                if (admin == null)
                {
                    result.Success = false;
                    result.Message = "Admin not found";
                    return result;
                }

                if (!string.IsNullOrEmpty(adminDtoObj.img))
                {
                    admin.img = adminDtoObj.img;
                }
                if (!string.IsNullOrEmpty(adminDtoObj.fullName))
                {
                    admin.User.FullName = adminDtoObj.fullName;
                }
                if (!string.IsNullOrEmpty(adminDtoObj.email))
                {
                    admin.User.Email = adminDtoObj.email;
                }
                if (!string.IsNullOrEmpty(adminDtoObj.password))
                {
                    admin.User.PasswordHash = adminDtoObj.password;
                }

                await dataAccessClassObj.SaveChangesAsync();

                result.Success = true;
                result.Message = "Admin profile updated successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}

