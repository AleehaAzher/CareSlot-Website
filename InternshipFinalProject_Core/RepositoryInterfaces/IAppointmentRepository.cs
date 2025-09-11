using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IAppointmentRepository
    {
        Task<List<AppointmentModel>> GetAllUsers();
        Task<UserCreationResult> Create(AppointmentViewModel appointmentObj);
        Task<List<AppointmentModel>> ApplyChanges(AppointmentApplyChanges filterObj);
        Task<AppointmentModel> GetUserById(int id);
        Task<UserCreationResult> Update(EditAppointmentViewModel appointmentObj);
        Task<UserCreationResult> Delete(int id);
        Task<int> AppointmentCount();
        Task<List<int>> AllAppointmentCount(int id);
        Task<List<AppointmentModel>> todaysAppointmentList(int id);
        Task<List<AppointmentModel>> getAppointmentRequests(int id);
        Task<int> getAppointmentRequestCount(int id);
        Task<int> getMyAppointmentCount(int id);
        Task<List<AppointmentModel>> PatientMyAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj, int? id);
        Task<List<AppointmentModel>> GetListByPatient(int id);
        Task<List<AppointmentModel>> GetListByDoctor(int id);
        Task<List<AppointmentModel>> DoctorAppointmentsApplyChanges(PatientMyAppointmentsApplyChanges filterObj, int? id);
    }
}
