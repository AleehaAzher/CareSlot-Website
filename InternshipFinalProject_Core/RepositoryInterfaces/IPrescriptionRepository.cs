using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.RepositoryInterfaces
{
    public interface IPrescriptionRepository
    {
        Task<UserCreationResult> SavePrescription(SavePrescriptionViewModel prescriptionObj);
        Task<List<PrescriptionModel>> GetAllPrescriptions(int id, string userType);
        Task<PrescriptionModel> GetPrescriptionDetailsById(int id);
        Task<int> PatientPrescriptionCount(int id);
    }
}
