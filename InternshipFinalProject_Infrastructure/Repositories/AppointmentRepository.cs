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
    public class AppointmentRepository: IAppointmentRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public AppointmentRepository(DataAccessClass dataAccessClassObj)
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<List<AppointmentModel>> GetAllUsers()
        {
            var data = await dataAccessClassObj.AppointmentTable
                   .Include(u => u.Doctor)
                   .ThenInclude(d => d.User)
                    .Include(u => u.Patient)
                    .ThenInclude(p => p.User)
                    .ToListAsync();
            return data;
        }
        public async Task<UserCreationResult> Create(AppointmentViewModel appointmentObj)
        {
            var userCreationResultObj = new UserCreationResult();

            var doctorExists = await dataAccessClassObj.DoctorTable
                .FirstOrDefaultAsync(x => x.DoctorId == appointmentObj.DoctorId);
            var patientExists = await dataAccessClassObj.PatientTable
                .FirstOrDefaultAsync(x => x.UserId == appointmentObj.PatientId);

            if (doctorExists == null || patientExists == null)
            {
                userCreationResultObj.Success = false;
                userCreationResultObj.Message = "Invalid Doctor or Patient. Appointment can't be created.";
                return userCreationResultObj;
            }
            var AppointmentObj = new AppointmentModel();
            if (appointmentObj.Status.HasValue)
            {
                AppointmentObj.Status = (AppointmentStatusEnum)appointmentObj.Status.Value;
            }
            AppointmentObj.DoctorId = appointmentObj.DoctorId;
            AppointmentObj.PatientId = patientExists.PatientId;
            AppointmentObj.Notes = appointmentObj.Notes;
            AppointmentObj.Date = appointmentObj.Date;
            AppointmentObj.AppointmentType = appointmentObj.AppointmentType;

            await dataAccessClassObj.AppointmentTable.AddAsync(AppointmentObj);
            await dataAccessClassObj.SaveChangesAsync();

            userCreationResultObj.Success = true;
            userCreationResultObj.Message = "Appointment created successfully.";
            return userCreationResultObj;
        }
        public async Task<List<AppointmentModel>> ApplyChanges(AppointmentApplyChanges filterObj)
        {
            var allAppointments = await dataAccessClassObj.AppointmentTable
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .ToListAsync();

            if (filterObj.DoctorId.HasValue)
            {
                allAppointments = allAppointments
                    .Where(a => a.DoctorId == filterObj.DoctorId.Value)
                    .ToList();
            }
            if (filterObj.PatientId.HasValue)
            {
                allAppointments = allAppointments
                    .Where(a => a.PatientId == filterObj.PatientId.Value)
                    .ToList();
            }
            if (filterObj.Date.HasValue)
            {
                var filterDate = filterObj.Date.Value.Date;
                allAppointments = allAppointments
                    .Where(a => a.Date.HasValue && a.Date.Value.Date == filterDate)
                    .ToList();
            }
            if (filterObj.Status.HasValue)
            {
                allAppointments = allAppointments
                    .Where(a => a.Status == filterObj.Status.Value)
                    .ToList();
            }
            return allAppointments;
        }
        public async Task<AppointmentModel> GetUserById(int id)
        {
            var allAppointments = await dataAccessClassObj.AppointmentTable
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                    .FirstOrDefaultAsync(x => x.AppointmentId == id);
            if (allAppointments == null)
            {
                return null;
            }
            return allAppointments;
        }
        public async Task<UserCreationResult>Update(EditAppointmentViewModel appointmentObj)
        {
            var userCreationResultObj = new UserCreationResult();
            var AppointmentObj = await dataAccessClassObj.AppointmentTable.FirstOrDefaultAsync(x => x.AppointmentId == appointmentObj.AppointmentId);
            if (appointmentObj.Status.HasValue)
            {
                AppointmentObj.Status = (AppointmentStatusEnum)appointmentObj.Status.Value;
            }
            AppointmentObj.DoctorId = appointmentObj.DoctorId;
            AppointmentObj.PatientId = appointmentObj.PatientId;
            AppointmentObj.Notes = appointmentObj.Notes;
            AppointmentObj.Date = appointmentObj.Date;
            AppointmentObj.UpdatedAt = DateTime.Now;
            AppointmentObj.AppointmentType = appointmentObj.AppointmentType;

            await dataAccessClassObj.SaveChangesAsync();

            userCreationResultObj.Success = true;
            userCreationResultObj.Message = "Appointment created successfully.";
            return userCreationResultObj;
        }
        public async Task<UserCreationResult> Delete(int id)
        {
            var result = new UserCreationResult();

            try
            {
                var appointment = await dataAccessClassObj.AppointmentTable.FindAsync(id);
                if (appointment == null)
                {
                    result.Success = false;
                    result.Message = "Appointment not found.";
                    return result;
                }

                dataAccessClassObj.AppointmentTable.Remove(appointment);
                await dataAccessClassObj.SaveChangesAsync();

                result.Success = true;
                result.Message = "Appointment deleted successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        public async Task<int> AppointmentCount()
        {
            var list = await dataAccessClassObj.AppointmentTable
                .ToListAsync();
            int count = 0;
            foreach (var item in list)
            {
                count++;
            }
            return count;
        }
        public async Task<List<int>> AllAppointmentCount(int id)
        {
            var todaysAppointment = await dataAccessClassObj.AppointmentTable
                .Where(x => x.Doctor.UserId == id &&
                            x.Date.HasValue &&
                            x.Date.Value.Date == DateTime.Today)
                .CountAsync();
            var pendingAppointment = await dataAccessClassObj.AppointmentTable
                .Where(x => x.Doctor.UserId == id &&
                            x.Status == AppointmentStatusEnum.Confirmed)
                .CountAsync();
            var completedAppointment = await dataAccessClassObj.AppointmentTable
                .Where(x => x.Doctor.UserId == id &&
                            x.Status == AppointmentStatusEnum.Completed)
                .CountAsync();
            var cancelledAppointment = await dataAccessClassObj.AppointmentTable
                .Where(x => x.Doctor.UserId == id &&
                            x.Status == AppointmentStatusEnum.Cancelled)
                .CountAsync();
            var confirmedPatientAppointment = await dataAccessClassObj.AppointmentTable
              .Where(x => x.Patient.UserId == id &&
                          x.Status == AppointmentStatusEnum.Confirmed)
              .CountAsync();
            var completedPatientAppointment = await dataAccessClassObj.AppointmentTable
              .Where(x => x.Patient.UserId == id &&
                          x.Status == AppointmentStatusEnum.Completed)
              .CountAsync();
            var list = new List<int>()
            {
                todaysAppointment,pendingAppointment,completedAppointment,cancelledAppointment,confirmedPatientAppointment,completedPatientAppointment
            };
            return list;
        }
        public async Task<List<AppointmentModel>> todaysAppointmentList(int id)
        {
            var list = await dataAccessClassObj.AppointmentTable
                .Include(x=>x.Patient).ThenInclude(x=>x.User)
                .Where(x => x.Doctor.UserId == id && x.Date.HasValue && x.Date.Value.Date==DateTime.Today)
                .ToListAsync();
            if(list==null)
            {
                return null;
            }
            return list;
        }
        public async Task<List<AppointmentModel>> getAppointmentRequests(int id)
        {
            var appointmentRequests = await dataAccessClassObj.AppointmentTable
                .Include(u => u.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Where(a => a.Doctor.UserId == id && a.Status == AppointmentStatusEnum.Pending)
                .ToListAsync();
            return appointmentRequests;
        }
        public async Task<int> getAppointmentRequestCount(int id)
        {
            var appointmentRequests = await dataAccessClassObj.AppointmentTable
                .Include(u => u.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Where(a => a.Doctor.UserId == id && a.Status == AppointmentStatusEnum.Pending)
                .CountAsync();
            return appointmentRequests;
        }
        public async Task<int> getMyAppointmentCount(int id)
        {
            var appointmentRequests = await dataAccessClassObj.AppointmentTable
                .Include(u => u.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Where(a => a.Patient.UserId == id && (a.Status == AppointmentStatusEnum.Confirmed || a.Status == AppointmentStatusEnum.Cancelled))
                .CountAsync();
            return appointmentRequests;
        }
        public async Task<List<AppointmentModel>> PatientMyAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj, int? id)
        {
            var allAppointments = await dataAccessClassObj.AppointmentTable
               .Include(a => a.Doctor).ThenInclude(d => d.User)
               .Include(a => a.Patient).ThenInclude(p => p.User)
               .ToListAsync();

           
            if (!string.IsNullOrEmpty(filterObj.Mode))
            {
                allAppointments = allAppointments
                    .Where(a => a.AppointmentType  == filterObj.Mode && a.Patient.UserId==id)
                    .ToList();
            }
            if (filterObj.Status.HasValue)
            {
                allAppointments = allAppointments
                    .Where(a => a.Status == filterObj.Status.Value && a.Patient.UserId == id)
                    .ToList();
            }
            if(string.IsNullOrEmpty(filterObj.Mode) && !filterObj.Status.HasValue)
            {
                allAppointments = allAppointments
                   .Where(a => a.Patient.UserId == id)
                   .ToList();
            }
            return allAppointments;
        }
        public async Task<List<AppointmentModel>>GetListByPatient(int id)
        {
            var data = await dataAccessClassObj.AppointmentTable
                 .Include(u => u.Doctor)
                 .ThenInclude(d => d.User)
                  .Include(u => u.Patient)
                  .ThenInclude(p => p.User)
                  .Where(a=>a.Patient.UserId==id)
                  .ToListAsync();
            return data;
        }
        public async Task<List<AppointmentModel>> GetListByDoctor(int id)
        {
            var data = await dataAccessClassObj.AppointmentTable
                 .Include(u => u.Doctor)
                 .ThenInclude(d => d.User)
                  .Include(u => u.Patient)
                  .ThenInclude(p => p.User)
                  .Where(a => a.Doctor.UserId == id)
                  .ToListAsync();
            return data;
        }
        public async Task<List<AppointmentModel>> DoctorAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj, int? id)
        {
            var allAppointments = await dataAccessClassObj.AppointmentTable
               .Include(a => a.Doctor).ThenInclude(d => d.User)
               .Include(a => a.Patient).ThenInclude(p => p.User)
               .ToListAsync();


            if (!string.IsNullOrEmpty(filterObj.Mode))
            {
                allAppointments = allAppointments
                    .Where(a => a.AppointmentType == filterObj.Mode && a.Doctor.UserId == id && a.Status!=AppointmentStatusEnum.Pending)
                    .ToList();
            }
            if (filterObj.Status.HasValue)
            {
                allAppointments = allAppointments
                    .Where(a => a.Status == filterObj.Status.Value && a.Doctor.UserId == id)
                    .ToList();
            }
            if (string.IsNullOrEmpty(filterObj.Mode) && !filterObj.Status.HasValue)
            {
                allAppointments = allAppointments
                   .Where(a => a.Doctor.UserId == id)
                   .ToList();
            }
            return allAppointments;
        }
    }
}
