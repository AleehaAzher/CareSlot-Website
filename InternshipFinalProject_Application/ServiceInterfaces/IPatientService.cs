using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.ServiceInterfaces
{
    public interface IPatientService
    {
        Task<List<PatientModel>> GetAll();
        Task<UserCreationResult> CreateUser(PatientCreateHelperClass patientObj);
        Task<PatientModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<UserCreationResult> UpdateUser(EditPatient patientModelObj);
        Task<UserCreationResult> DeleteUser(int id);
        Task<int> PatientCount();
        Task<UserCreationResult> updatePatient(PatientDTO patientDTO);
        Task<UserCreationResult> UpdateStatus(int id, string status);

    }
}
