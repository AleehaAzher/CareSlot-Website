using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IDoctorRepository
    {
        Task<DoctorModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<UserCreationResult> updateDoctorProfile(DoctorDTO doctorDTO);
        Task<List<DoctorModel>> GetByPendingStatus();
        Task<List<DoctorModel>> GetAll();
        Task<UserCreationResult> UpdateStatus(int id, string status);
        Task<int> DoctorCount();
        Task<List<DoctorModel>> ApplyChanges(DoctorApplyChanges filterObj);
        Task<List<DoctorModel>> BookAppointmentApplyFilters(BookAppointmentApplyFilters filterObj);

    }
}
