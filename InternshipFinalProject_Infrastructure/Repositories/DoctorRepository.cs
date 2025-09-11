using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using InternshipFinalProject_Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Infrastructure.Repositories
{
    public class DoctorRepository:IDoctorRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public DoctorRepository(DataAccessClass dataAccessClassObj)
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<DoctorModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            if (id == null && email != null)
            {
                return await dataAccessClassObj.DoctorTable
                    .Include(a => a.User)
                    .Include(a=>a.AvailableDays)
                       .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.User.Email == email);

            }
            else if (id != null && email == null)
            {
                var data = await dataAccessClassObj.DoctorTable
                    .Include(x => x.User)
                    .Include(a => a.AvailableDays)
                    .FirstOrDefaultAsync(x => x.UserId == id);
                return data;
            }
            else
            {
                return null;
            }
        }
        public async Task<UserCreationResult> updateDoctorProfile(DoctorDTO doctorDTO)
        {
            var result = new UserCreationResult();

            try
            {
                var doctor = await dataAccessClassObj.DoctorTable
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.UserId == doctorDTO.id);

                if (doctor == null)
                {
                    result.Success = false;
                    result.Message = "Doctor not found";
                    return result;
                }

                if (!string.IsNullOrEmpty(doctorDTO.img))
                {
                    doctor.img = doctorDTO.img;
                }

                if (!string.IsNullOrEmpty(doctorDTO.fullName))
                {
                    doctor.User.FullName = doctorDTO.fullName;
                }

                if (!string.IsNullOrEmpty(doctorDTO.email))
                {
                    doctor.User.Email = doctorDTO.email;
                }

                if (!string.IsNullOrEmpty(doctorDTO.password))
                {
                    doctor.User.PasswordHash = doctorDTO.password;
                }

                if (!string.IsNullOrEmpty(doctorDTO.Qualifications))
                {
                    doctor.Qualifications = doctorDTO.Qualifications;
                }

                if (!string.IsNullOrEmpty(doctorDTO.Specialization))
                {
                    doctor.Specialization = doctorDTO.Specialization;
                }

                if (doctorDTO.ExperienceYears.HasValue)
                {
                    doctor.ExperienceYears = doctorDTO.ExperienceYears.Value;
                }

                if (doctorDTO.AvailableDays != null && doctorDTO.AvailableDays.Any())
                {
                    var existingDays = dataAccessClassObj.DoctorAvailableDaysTable
                                       .Where(d => d.DoctorId == doctor.DoctorId);

                    dataAccessClassObj.DoctorAvailableDaysTable.RemoveRange(existingDays);
                    foreach (var day in doctorDTO.AvailableDays)
                    {
                        doctor.AvailableDays.Add(new DoctorAvailableDays
                        {
                            Day = day,
                            DoctorId = doctor.DoctorId
                        });
                    }

                    //doctor.AvailableDays = doctorDTO.AvailableDays.Value;
                }

                if (doctorDTO.AvailableFromTime.HasValue)
                {
                    doctor.AvailableFromTime = doctorDTO.AvailableFromTime.Value;
                }

                if (doctorDTO.AvailableToTime.HasValue)
                {
                    doctor.AvailableToTime = doctorDTO.AvailableToTime.Value;
                }

                if (doctorDTO.Fees.HasValue)
                {
                    doctor.Fees = doctorDTO.Fees.Value;
                }

                if (!string.IsNullOrEmpty(doctorDTO.Contact))
                {
                    doctor.Contact = doctorDTO.Contact;
                }

                if (!string.IsNullOrEmpty(doctorDTO.Gender))
                {
                    doctor.Gender = doctorDTO.Gender;
                }

                if (!string.IsNullOrEmpty(doctorDTO.Bio))
                {
                    doctor.Bio = doctorDTO.Bio;
                }

                await dataAccessClassObj.SaveChangesAsync();

                result.Success = true;
                result.Message = "Doctor profile updated successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        public async Task<List<DoctorModel>> GetByPendingStatus()
        {
            var doctors = await dataAccessClassObj.DoctorTable
                .Include(d => d.User)
                .Where(d => d.ApprovalStatus == "Pending")
                .ToListAsync();

            return doctors.Where(IsProfileComplete).ToList();
        }
        private bool IsProfileComplete(DoctorModel d)
        {
            if (string.IsNullOrWhiteSpace(d.User?.FullName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.User?.Email))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.Qualifications))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.Specialization))
            {
                return false;
            }

            if (d.ExperienceYears == null || d.ExperienceYears <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.Contact))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.Gender))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.Bio))
            {
                return false;
            }

            if (d.Fees == null || d.Fees <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(d.img))
            {
                return false;
            }

            if (d.AvailableDays == null)
            {
                return false;
            }

            if (d.AvailableFromTime == null)
            {
                return false;
            }

            if (d.AvailableToTime == null)
            {
                return false;
            }

            return true;
        }
        public async Task<List<DoctorModel>> GetAll()
        {
            var doctors = await dataAccessClassObj.DoctorTable
                .Include(d => d.User)
              .Include(d => d.AvailableDays)
                .ToListAsync();

            return doctors;
        }
        public async Task<UserCreationResult> UpdateStatus(int id, string status)
        {
            var rslt = new UserCreationResult();
            var doctor = await dataAccessClassObj.DoctorTable
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == id && a.ApprovalStatus == "Pending");
            if (doctor == null)
            {
                rslt.Success = false;
                rslt.Message = "Error finding doctor";
                return rslt;
            }
            if (doctor != null && status=="Approved")
            {
                doctor.ApprovalStatus = status;
                doctor.User.IsActive = true;
            }
            if (doctor != null && status == "Rejected")
            {
                doctor.ApprovalStatus = status;
                doctor.User.IsActive = false;
            }
            rslt.Success = true;
            rslt.Message = "Doctor status updated successfully";
            await dataAccessClassObj.SaveChangesAsync();
            return rslt;
        }
        public async Task<int> DoctorCount()
        {
            var list = await dataAccessClassObj.DoctorTable
                .Include(d => d.User)
                .Where(u=>u.User.IsActive==true)
                .ToListAsync();
            int count = 0;
            foreach (var item in list)
            {
                count++;
            }
            return count;
        }
        public async Task<List<DoctorModel>> ApplyChanges(DoctorApplyChanges filterObj)
        {
            var allDoctors = await dataAccessClassObj.DoctorTable
                .Include(a => a.User)
                    .Include(a => a.AvailableDays)
                .ToListAsync();

            if (filterObj.Specialization!=null)
            {
                allDoctors = allDoctors
                    .Where(a => a.Specialization == filterObj.Specialization)
                    .ToList();
            }
            if (filterObj.Status!=null)
            {
                allDoctors = allDoctors
                    .Where(a => a.ApprovalStatus == filterObj.Status)
                    .ToList();
            }
            if (!string.IsNullOrEmpty(filterObj.Day))
            {
                allDoctors = allDoctors
                    .Where(a => a.AvailableDays.Any(d => d.Day == filterObj.Day))
                    .ToList();
            }
            if(!string.IsNullOrEmpty(filterObj.Account))
            {
                if(filterObj.Account=="Active")
                {
                    allDoctors = allDoctors
                        .Where(a => a.User.IsActive == true)
                        .ToList();
                }
                if(filterObj.Account=="Deactivated")
                {
                    allDoctors = allDoctors
                        .Where(a => a.User.IsActive == false)
                        .ToList();
                }
            }
            return allDoctors;
        }
        public async Task<List<DoctorModel>> BookAppointmentApplyFilters(BookAppointmentApplyFilters filterObj)
        {
            var allDoctors = await dataAccessClassObj.DoctorTable
                .Include(a => a.User)
                    .Include(a => a.AvailableDays)
                    .Where(a=>a.User.IsActive==true)
                .ToListAsync();
            if (filterObj.Specialization != null)
            {
                allDoctors = allDoctors
                    .Where(a => a.Specialization == filterObj.Specialization)
                    .ToList();
            }
            if (!string.IsNullOrEmpty(filterObj.Day))
            {
               
                allDoctors = allDoctors
                    .Where(a => a.AvailableDays.Any(d => d.Day == filterObj.Day))
                    .ToList();
            }
            if(filterObj.Fees!=null)
            {
                allDoctors = allDoctors
                   .Where(a => a.Fees<=filterObj.Fees)
                   .ToList();
            }
            return allDoctors;
        }
    }
}
