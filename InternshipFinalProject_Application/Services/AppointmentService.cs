using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.Services
{
    public class AppointmentService:IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepo;

        public AppointmentService(IAppointmentRepository appointmentRepo)
        {
            this.appointmentRepo = appointmentRepo;
        }
        public async Task<List<AppointmentModel>> ReadUsers()
        {
            return await appointmentRepo.GetAllUsers();
        }
        public async Task<UserCreationResult> CreateUser(AppointmentViewModel appointmentObj)
        {
            return await appointmentRepo.Create(appointmentObj);
        }
        public async Task<List<AppointmentModel>> ApplyChanges(AppointmentApplyChanges appointmentObj)
        {
            return await appointmentRepo.ApplyChanges(appointmentObj);
        }
        public async Task<AppointmentModel> GetUserById(int id)
        {
            return await appointmentRepo.GetUserById(id);
        }
        public async Task<UserCreationResult>Update(EditAppointmentViewModel appointmentObj)
        {
            return await appointmentRepo.Update(appointmentObj);
        }
        public async Task<UserCreationResult> DeleteUser(int id)
        {
            var data = await appointmentRepo.Delete(id);
            return data;
        }
        public async Task<int> AppointmentCount()
        {
            var count = await appointmentRepo.AppointmentCount();
            return count;
        }
        public async Task<List<int>> AllAppointmentCount(int id)
        {
            return await appointmentRepo.AllAppointmentCount(id);
        }
        public async Task<List<AppointmentModel>> todaysAppointmentList(int id)
        {
            return await appointmentRepo.todaysAppointmentList(id);
        }
        public async Task<List<AppointmentModel>> getAppointmentRequests(int id)
        {
            return await appointmentRepo.getAppointmentRequests(id);
        }
        public async Task<int> getAppointmentRequestCount(int id)
        {
            return await appointmentRepo.getAppointmentRequestCount(id);
        }
        public async Task<int> getMyAppointmentCount(int id)
        {
            return await appointmentRepo.getMyAppointmentCount(id);
        }
       public async Task<List<AppointmentModel>> PatientMyAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj,int?id)
        {
            return await appointmentRepo.PatientMyAppointmentsApplyChanges(filterObj,id);
        }
        public async Task<List<AppointmentModel>> GetListByPatient(int id)
        {
            return await appointmentRepo.GetListByPatient(id);
        }
        public async Task<List<AppointmentModel>> GetListByDoctor(int id)
        {
            return await appointmentRepo.GetListByDoctor(id);
        }
        public async Task<List<AppointmentModel>> DoctorAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj, int? id)
        {
            return await appointmentRepo.DoctorAppointmentsApplyChanges(filterObj, id);
        }
    }
}
