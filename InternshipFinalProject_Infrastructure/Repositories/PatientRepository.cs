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
    public class PatientRepository:IPatientRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public PatientRepository(DataAccessClass dataAccessClassObj)
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<List<PatientModel>> GetAll()
        {
            var patients = await dataAccessClassObj.PatientTable
                .Include(d => d.User)
                .Where(u=>u.User.IsActive==true)
                .ToListAsync();

            return patients;
        }
        public async Task<UserCreationResult> Create(PatientCreateHelperClass patientObj)
        {
            var userCreationResultObj = new UserCreationResult();

            var alreadyExists = await dataAccessClassObj.UserTable
                                        .FirstOrDefaultAsync(x => x.Email == patientObj.Email);
            if (alreadyExists != null)
            {
                userCreationResultObj.Success = false;
                userCreationResultObj.Message = "Email already exists.";
                return userCreationResultObj;
            }

            var userModelObj = new UserModel
            {
                FullName = patientObj.FullName,
                Email = patientObj.Email,
                PasswordHash = patientObj.Password,
                Role = "Patient"
            };

            await dataAccessClassObj.UserTable.AddAsync(userModelObj);
            await dataAccessClassObj.SaveChangesAsync();  

            var patientModelObj = new PatientModel
            {
                UserId = userModelObj.UserId,
                Age = patientObj.Age,
                BloodGroup = patientObj.BloodGroup,
                Gender = patientObj.Gender,
                Contact = patientObj.Contact,
                EmergencyContactName = patientObj.EmergencyContactName,
                EmergencyContact = patientObj.EmergencyContact,
                Address = patientObj.Address
            };

            await dataAccessClassObj.PatientTable.AddAsync(patientModelObj);
            await dataAccessClassObj.SaveChangesAsync();  

            //var appointment = new AppointmentModel
            //{
            //    PatientId = patientModelObj.PatientId,
            //    Status = AppointmentStatusEnum.Pending,
            //    Notes = "No notes added yet"
            //};

            //await dataAccessClassObj.AppointmentTable.AddAsync(appointment);
            //await dataAccessClassObj.SaveChangesAsync();

            userCreationResultObj.Success = true;
            userCreationResultObj.Message = "Patient created successfully.";
            return userCreationResultObj;
        }
        public async Task<PatientModel> GetUserByIdOrEmail(int? id = null, string email = null)
        {
            if (id == null && email != null)
            {
                return await dataAccessClassObj.PatientTable
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(x => x.User.Email == email);

            }
            else if (id != null && email == null)
            {
                var data = await dataAccessClassObj.PatientTable
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == id);
                return data;
            }
            else
            {
                return null;
            }
        }
        public async Task<UserCreationResult> Update(EditPatient patientModelObj)
        {
            var result = new UserCreationResult();
            var existingUser = await dataAccessClassObj.PatientTable
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == patientModelObj.UserId);

            if (existingUser == null)
            {
                result.Success = false;
                result.Message = "Updation failed";
                return result;
            }
            existingUser.User.FullName = patientModelObj.FullName;
            existingUser.User.Email = patientModelObj.Email;
            if (!string.IsNullOrEmpty(patientModelObj.Password))
            {
                existingUser.User.PasswordHash = patientModelObj.Password;
            }
            existingUser.Contact = patientModelObj.Contact;
            existingUser.Gender = patientModelObj.Gender;
            existingUser.Age = patientModelObj.Age;
            existingUser.BloodGroup = patientModelObj.BloodGroup;
            existingUser.Address = patientModelObj.Address;
            existingUser.EmergencyContactName = patientModelObj.EmergencyContactName;
            existingUser.EmergencyContact = patientModelObj.EmergencyContact;
            existingUser.User.FullName = patientModelObj.FullName;
            existingUser.User.Email = patientModelObj.Email;
            existingUser.UserId = patientModelObj.UserId;
            result.Success = true;
            result.Message = "Successfully updated";
            await dataAccessClassObj.SaveChangesAsync();

            return result;
        }
        public async Task<UserCreationResult> Delete(int id)
        {
            var result = new UserCreationResult();

            try
            {
                var user = await dataAccessClassObj.UserTable.FindAsync(id);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "User not found.";
                    return result;
                }

                var patientRecord = await dataAccessClassObj.PatientTable
                    .FirstOrDefaultAsync(p => p.UserId == id);

                if (patientRecord != null)
                {
                    user.IsActive = false;
                }

                await dataAccessClassObj.SaveChangesAsync();

                result.Success = true;
                result.Message = "Patient deleted successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        public async Task<int> PatientCount()
        {
            var list = await dataAccessClassObj.PatientTable
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
        public async Task<UserCreationResult> updatePatientProfile(PatientDTO patientDTO)
        {
            var result = new UserCreationResult();

            try
            {
                var patient = await dataAccessClassObj.PatientTable
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.UserId == patientDTO.id);

                if (patient == null)
                {
                    result.Success = false;
                    result.Message = "Patient not found";
                    return result;
                }

                if (!string.IsNullOrEmpty(patientDTO.img))
                {
                    patient.img = patientDTO.img;
                }

                if (!string.IsNullOrEmpty(patientDTO.fullName))
                {
                    patient.User.FullName = patientDTO.fullName;
                }

                if (!string.IsNullOrEmpty(patientDTO.email))
                {
                    patient.User.Email = patientDTO.email;
                }

                if (!string.IsNullOrEmpty(patientDTO.password))
                {
                    patient.User.PasswordHash = patientDTO.password;
                }

                if (patientDTO.Age.HasValue)
                {
                    patient.Age = patientDTO.Age.Value;
                }

                if (!string.IsNullOrEmpty(patientDTO.Contact))
                {
                    patient.Contact = patientDTO.Contact;
                }

                if (!string.IsNullOrEmpty(patientDTO.Gender))
                {
                    patient.Gender = patientDTO.Gender;
                }

                if (!string.IsNullOrEmpty(patientDTO.Address))
                {
                    patient.Address = patientDTO.Address;
                }

                if (!string.IsNullOrEmpty(patientDTO.EmergencyContact))
                {
                    patient.EmergencyContact = patientDTO.EmergencyContact;
                }
                if (!string.IsNullOrEmpty(patientDTO.EmergencyContactName))
                {
                    patient.EmergencyContactName = patientDTO.EmergencyContactName;
                }
                if (!string.IsNullOrEmpty(patientDTO.BloodGroup))
                {
                    patient.BloodGroup = patientDTO.BloodGroup;
                }
                await dataAccessClassObj.SaveChangesAsync();

                result.Success = true;
                result.Message = "Patient profile updated successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        public async Task<UserCreationResult> UpdateStatus(int id, string status)
        {
            var rslt = new UserCreationResult();
            if (status == "Completed")
            {
                var response = await dataAccessClassObj.AppointmentTable
              .FirstOrDefaultAsync(a => a.AppointmentId == id);
                response.Status = AppointmentStatusEnum.Completed;
                rslt.Success = true;
                rslt.Message = "Appointment status updated successfully";
                await dataAccessClassObj.SaveChangesAsync();
                return rslt;
            }
            var appointment = await dataAccessClassObj.AppointmentTable
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.Status == AppointmentStatusEnum.Pending);
            if (appointment == null)
            {
                rslt.Success = false;
                rslt.Message = "Error finding appointment";
                return rslt;
            }
            if (appointment != null && status == "Confirmed")
            {
                appointment.Status = AppointmentStatusEnum.Confirmed;
            }
            if (appointment != null && status == "Cancelled")
            {
                appointment.Status = AppointmentStatusEnum.Cancelled;
            }
           
            rslt.Success = true;
            rslt.Message = "Appointment status updated successfully";
            await dataAccessClassObj.SaveChangesAsync();
            return rslt;
        }
    }
}
