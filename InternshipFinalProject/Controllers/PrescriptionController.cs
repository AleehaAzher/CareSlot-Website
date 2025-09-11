using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipFinalProject.Controllers
{
    public class PrescriptionController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPrescriptionService prescriptionService;
        private readonly IPatientService patientService;

        public PrescriptionController(IAppointmentService appointmentService,IPrescriptionService prescriptionService,IPatientService patientService)
        {
            this.appointmentService = appointmentService;
            this.prescriptionService = prescriptionService;
            this.patientService = patientService;
        }
        public async Task<IActionResult> StartNow(int id)
        {
            var userId = int.Parse(GetCurrentUserId());
            var response = await appointmentService.GetUserById(id);
           
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> SavePrescription([FromBody] SavePrescriptionViewModel prescriptionObj)
        {
            if(!ModelState.IsValid)
            {
                return Json(new
                {
                    message="Invalid model state",
                    success=true
                });
            }
            var response = await prescriptionService.SavePrescription(prescriptionObj);
         
            if(response==null)
            {
                 return Json(new
                {
                    message=response.Message,
                    success=false
                }); 
            }
            var response2 = await patientService.UpdateStatus(prescriptionObj.AppointmentId, "Completed");
            if(response2==null)
            {
                return Json(new
                {
                    message = response.Message,
                    success = false
                });
            }
            return Json(new
            {
                success =true,
                mesage=response.Message
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetPrescriptionDetailsById(int id)
        {
            var response = await prescriptionService.GetPrescriptionDetailsById(id);
            if(response==null)
            {
                return Json(new
                {
                    message = "Error getting prescription details",
                    success = false
                });
            }
            var medicines = new List<object>();
            foreach (var med in response.PrescriptionDetails)
            {
                medicines.Add(new
                {
                    MedicineName = med.MedicineName,
                    Dosage = med.Dosage,
                    Form = med.Form,
                    Duration = med.Duration
                });
            }

            return Json(new
            {
                message = "View details fetched",
                success = true,
                doctorName=response.Doctor.User.FullName,
                patientName=response.Patient.User.FullName,
                prescribedDate=response.PrescribedDate,
                diagnosis=response.Diagnosis,
                advice=response.Advice,
                medicines=medicines
            });
        }
        [HttpGet]
        public async Task<IActionResult> PatientPrescriptionCount(int id)
        {
            var response = await prescriptionService.PatientPrescriptionCount(id);
            return Json(new
            {
                success = true,
                message = "Today's Appointment Count fetched",
                count = response
               
            });
        }
    }
}
