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
    public class PatientService:IPatientService
    {
        private readonly IPatientRepository patientRepo;

        public PatientService(IPatientRepository patientRepo)
        {
            this.patientRepo = patientRepo;
        }
        public async Task<List<PatientModel>> GetAll()
        {
            var list = await patientRepo.GetAll();
            return list;
        }
        public async Task<UserCreationResult> CreateUser(PatientCreateHelperClass patientObj)
        {
            patientObj.Password = HashPassword(patientObj.Password);
            return await patientRepo.Create(patientObj);
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
        public async Task<PatientModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            return await patientRepo.GetUserByIdOrEmail(id, email);
        }
        public async Task<UserCreationResult> UpdateUser(EditPatient patientModelObj)
        {
            if (!string.IsNullOrEmpty(patientModelObj.Password))
            {
                patientModelObj.Password = HashPassword(patientModelObj.Password);
            }

            var updatedUser = await patientRepo.Update(patientModelObj);
            return updatedUser;
        }
        public async Task<UserCreationResult> DeleteUser(int id)
        {
            var data = await patientRepo.Delete(id);
            return data;
        }
        public async Task<int> PatientCount()
        {
            var count = await patientRepo.PatientCount();
            return count;
        }
        public async Task<UserCreationResult> updatePatient(PatientDTO patientDTO)
        {
            if (!string.IsNullOrEmpty(patientDTO.password))
            {
                patientDTO.password = HashPassword(patientDTO.password);
            }

            return await patientRepo.updatePatientProfile(patientDTO);
        }
        public async Task<UserCreationResult> UpdateStatus(int id, string status)
        {
            var result = await patientRepo.UpdateStatus(id, status);
            return result;
        }
    }
}
