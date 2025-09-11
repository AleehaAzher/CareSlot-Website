using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IPatientRepository
    {
        Task<List<PatientModel>> GetAll();
        Task<UserCreationResult> Create(PatientCreateHelperClass patientObj);
        Task<PatientModel> GetUserByIdOrEmail(int? id = null, string email = null);
        Task<UserCreationResult> Update(EditPatient patientModelObj);
        Task<UserCreationResult> Delete(int id);
        Task<int> PatientCount();
        Task<UserCreationResult> updatePatientProfile(PatientDTO patientDTO);
        Task<UserCreationResult> UpdateStatus(int id, string status);
    }
}
