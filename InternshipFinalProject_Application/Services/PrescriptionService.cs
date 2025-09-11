using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.Services
{
    public class PrescriptionService:IPrescriptionService
    {
        private readonly IPrescriptionRepository prescriptionRepo;

        public PrescriptionService(IPrescriptionRepository prescriptionRepo) 
        {
            this.prescriptionRepo = prescriptionRepo;
        }
        public async Task<UserCreationResult> SavePrescription(SavePrescriptionViewModel prescriptionObj)
        {
            return await prescriptionRepo.SavePrescription(prescriptionObj);
        }
        public async Task<List<PrescriptionModel>> GetAllPrescriptions(int id,string userType)
        {
            return await prescriptionRepo.GetAllPrescriptions(id,userType);
        }
        public async Task<PrescriptionModel> GetPrescriptionDetailsById(int id)
        {
            return await prescriptionRepo.GetPrescriptionDetailsById(id);
        }
        public async Task<int> PatientPrescriptionCount(int id)
        {
            return await prescriptionRepo.PatientPrescriptionCount(id);
        }
    }
}
