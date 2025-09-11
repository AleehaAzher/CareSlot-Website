using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using InternshipFinalProject_Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly DataAccessClass dataAccessObj;

        public UserRepository(DataAccessClass dataAccessObj)
        {
            this.dataAccessObj = dataAccessObj;
        }
        public async Task<UserCreationResult> Create(UserModel userModelObj)
        {
            var userCreationResultObj=new UserCreationResult();
            var alreadyExit = await dataAccessObj.UserTable
                .FirstOrDefaultAsync(x => x.Email == userModelObj.Email);

            if (alreadyExit != null)
            {
                userCreationResultObj.Success = false;
                userCreationResultObj.Message = "Email already exists.";
                return userCreationResultObj;
            }

            if (userModelObj.Role == "Doctor" || userModelObj.Role == "Patient")
            {
                await dataAccessObj.UserTable.AddAsync(userModelObj);
                await dataAccessObj.SaveChangesAsync();

                if (userModelObj.Role == "Doctor")
                {
                    var doctor = new DoctorModel
                    {
                        UserId = userModelObj.UserId
                    };
                    await dataAccessObj.DoctorTable.AddAsync(doctor);
                    //var doctorDays = new DoctorAvailableDays
                    //{
                    //    AvailabledaysId = userModelObj.Doctor.DoctorId
                    //};
                    //await dataAccessObj.DoctorAvailableDaysTable.AddAsync(doctorDays);
                    await dataAccessObj.SaveChangesAsync();

                    //var appointment = new AppointmentModel
                    //{
                    //    DoctorId = doctor.DoctorId,
                    //    PatientId = null,
                    //};
                    //await dataAccessObj.AppointmentTable.AddAsync(appointment);
                }
                else if (userModelObj.Role == "Patient")
                {
                    var patient = new PatientModel
                    {
                        UserId = userModelObj.UserId
                    };
                    await dataAccessObj.PatientTable.AddAsync(patient);
                    await dataAccessObj.SaveChangesAsync();

                    //var appointment = new AppointmentModel
                    //{
                    //    PatientId = patient.PatientId,
                    //    DoctorId = null,
                    //};
                    //await dataAccessObj.AppointmentTable.AddAsync(appointment);
                }

                await dataAccessObj.SaveChangesAsync();

                userCreationResultObj.Success = true;
                userCreationResultObj.Message = "User created successfully with appointment record.";
            }

            return userCreationResultObj;
        }
        public async Task<UserModel?> GetUserByEmail(string email, string password)
        {
            var data = await dataAccessObj.UserTable.FirstOrDefaultAsync(x => x.Email == email);
            if (data == null) return null;
            var hashedPassword = HashPassword(password);
            if (hashedPassword == data.PasswordHash)
            {
                return data;
            }
            return null;
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
        public async Task<UserModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            if (id == null && email != null)
            {
                return await dataAccessObj.UserTable.FirstOrDefaultAsync(x => x.Email == email);
            }
            else if (id != null && email == null)
            {
                var data = await dataAccessObj.UserTable.FindAsync(id);
                return data;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<UserModel>> GetAllUsers()
        {
            var data = await dataAccessObj.UserTable
                   .Include(u => u.Doctor)
                    .Include(u => u.Patient)
                    .Where(u=>u.IsActive==true)
                    .ToListAsync();
            return data;
        }
        public async Task<UserCreationResult> Update(UserModel userModelObj)
        {
            var result = new UserCreationResult();
            var existingUser = await dataAccessObj.UserTable.FindAsync(userModelObj.UserId);
            if (existingUser == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }
            existingUser.FullName = userModelObj.FullName;
            existingUser.Email = userModelObj.Email;
            existingUser.PasswordHash = userModelObj.PasswordHash;
            existingUser.Role = userModelObj.Role;
            existingUser.ResetToken = userModelObj.ResetToken;
            existingUser.TokenExpiry = userModelObj.TokenExpiry;

            await dataAccessObj.SaveChangesAsync();
            result.Success = true;
            result.Message = "User updated";
            return result;
        }
    }
}
