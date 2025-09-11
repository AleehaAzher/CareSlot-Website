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
    public class DoctorService:IDoctorService
    {
        private readonly IDoctorRepository doctorRepo;

        public DoctorService(IDoctorRepository doctorRepo)
        {
            this.doctorRepo = doctorRepo;
        }
        public async Task<DoctorModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            return await doctorRepo.GetUserByIdOrEmail(id, email);
        }
        public async Task<UserCreationResult> updateDoctor(DoctorDTO doctorDTO)
        {
            if (!string.IsNullOrEmpty(doctorDTO.password))
            {
                doctorDTO.password = HashPassword(doctorDTO.password);
            }

            return await doctorRepo.updateDoctorProfile(doctorDTO);
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
        public async Task<List<DoctorModel>> GetByPendingStatus()
        {
            var list = await doctorRepo.GetByPendingStatus();
            return list;
        }
        public async Task<List<DoctorModel>> GetAll()
        {
            var list = await doctorRepo.GetAll();
            return list;
        }
        public async Task<UserCreationResult> UpdateStatus(int id, string status)
        {
            var result = await doctorRepo.UpdateStatus(id, status);
            return result;
        }
        public async Task<int> DoctorCount()
        {
            var count = await doctorRepo.DoctorCount();
            return count;
        }
        public async Task<List<DoctorModel>> ApplyChanges(DoctorApplyChanges doctorObj)
        {
            return await doctorRepo.ApplyChanges(doctorObj);
        }
        public async Task<List<DoctorModel>> BookAppointmentApplyFilters(BookAppointmentApplyFilters filterObj)
        {
            return await doctorRepo.BookAppointmentApplyFilters(filterObj);
        }
    }
}
