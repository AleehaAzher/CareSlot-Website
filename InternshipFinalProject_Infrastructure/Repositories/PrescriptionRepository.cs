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
    public class PrescriptionRepository:IPrescriptionRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public PrescriptionRepository(DataAccessClass dataAccessClassObj) 
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<UserCreationResult> SavePrescription(SavePrescriptionViewModel prescriptionObj)
        {
            var result = new UserCreationResult();

            try
            {
                if (prescriptionObj == null)
                {
                    result.Success = false;
                    result.Message = "Prescription data is missing.";
                    return result;
                }

                var prescription = new PrescriptionModel
                {
                    PrescribedDate = DateTime.Now,
                    Diagnosis = prescriptionObj.Diagnosis,
                    Advice = prescriptionObj.Advice,
                    DoctorId = prescriptionObj.DoctorId,
                    PatientId = prescriptionObj.PatientId
                };

                await dataAccessClassObj.Prescriptiontable.AddAsync(prescription);
                await dataAccessClassObj.SaveChangesAsync();

                if (prescriptionObj.PrescriptionDetailsColumn != null && prescriptionObj.PrescriptionDetailsColumn.Any())
                {
                    foreach (var item in prescriptionObj.PrescriptionDetailsColumn)
                    {
                        var detail = new PrescriptionDetailsModel
                        {
                            MedicineName = item.MedicineName,
                            Dosage = item.Dosage,
                            Form = item.Form,
                            Duration = item.Duration,
                            PrescriptionId = prescription.PrescriptionId
                        };
                        await dataAccessClassObj.PrescriptionDetailsTable.AddAsync(detail); 
                    }

                    await dataAccessClassObj.SaveChangesAsync();
                }

                result.Success = true;
                result.Message = "Prescription saved successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        public async Task<List<PrescriptionModel>> GetAllPrescriptions(int id,string userType)
        {
            var prescriptionList = new List<PrescriptionModel>();
            if (userType=="Patient")
            {
                prescriptionList = await dataAccessClassObj.Prescriptiontable
              .Include(a => a.PrescriptionDetails)
              .Include(a => a.Doctor).ThenInclude(a=>a.User)
              .Include(a => a.Patient).ThenInclude(a => a.User)
              .Where(a => a.Patient.UserId ==id)
              .ToListAsync();
            }
            if (userType == "Doctor")
            {
               prescriptionList = await dataAccessClassObj.Prescriptiontable
              .Include(a => a.PrescriptionDetails)
              .Include(a => a.Doctor).ThenInclude(a => a.User)
              .Include(a => a.Patient).ThenInclude(a => a.User)
              .Where(a => a.Doctor.UserId == id)
              .ToListAsync();
            }
            return prescriptionList;
        }
        public async Task<PrescriptionModel> GetPrescriptionDetailsById(int id)
        {
            var prescription = await dataAccessClassObj.Prescriptiontable
            .Include(a => a.PrescriptionDetails)
            .Include(a => a.Doctor).ThenInclude(a => a.User)
            .Include(a => a.Patient).ThenInclude(a => a.User)
            .FirstOrDefaultAsync(a => a.PrescriptionId == id);
            return prescription;
        }

        public async Task<int> PatientPrescriptionCount(int id)
        {
            var count = await dataAccessClassObj.Prescriptiontable
            .Include(a => a.PrescriptionDetails)
            .Include(a => a.Doctor).ThenInclude(a => a.User)
            .Include(a => a.Patient).ThenInclude(a => a.User)
            .Where(a => a.Patient.UserId == id)
            .CountAsync();
            return count;
        }
    }
}
