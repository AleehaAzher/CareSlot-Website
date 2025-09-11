using Azure.Core;
using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Application.Services;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InternshipFinalProject.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IUserService userService;
        private readonly IPatientService patientService;
        string ReasonForRejectingAppointment;

        public AppointmentController(IAppointmentService appointmentService,IUserService userService,IPatientService patientService)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
            this.patientService = patientService;
        }
        public async Task<IActionResult> ManageAppointments()
        {
            List<UserModel> list = new List<UserModel>();
            var response = await userService.ReadUsers();
            if (response!=null)
            {
                list = response;
            }

            List<AppointmentModel> appointmentlist = new List<AppointmentModel>();
            var response2 = await appointmentService.ReadUsers();
            if (response2!=null)
            {
                appointmentlist = response2;
                
            }
            ViewBag.appointmentList = appointmentlist;
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentViewModel appointmentObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            if (appointmentObj.Status.HasValue)
            {
                appointmentObj.Status = (AppointmentStatusEnum)appointmentObj.Status.Value;
            }
            var response = await appointmentService.CreateUser(appointmentObj);
            if (response!=null)
            {
                return Json(new
                {
                    success = true,
                    message = "Appointment created successfully",
                });
            }
            return Json(new { success = false, message = "Failed to create appointment" });
        }
        [HttpPost]
        public async Task<ActionResult> ApplyChanges([FromBody] AppointmentApplyChanges appointmentObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            if (appointmentObj.Status.HasValue)
            {
                appointmentObj.Status = (AppointmentStatusEnum)appointmentObj.Status.Value;
            }
            var response = await appointmentService.ApplyChanges(appointmentObj);
            List<AppointmentModel> appointmentlist = new List<AppointmentModel>();

            if (response!=null)
            {
                    appointmentlist = response;
            }
            return PartialView("AppointmentListPartialView", appointmentlist);
        }
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int id)
        {
            var response = await appointmentService.GetUserById(id);
            if (response==null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error loading details of appointment"
                });
            }
            return Json(new
            {
                success = true,
                message = "Success loading details of appointment",
                doctorName=response.Doctor.User.FullName,
                patientName=response.Patient.User.FullName,
                date=response.Date,
                status=response.Status,
                notes=response.Notes,
                createdAt=response.CreatedAt,
                updatedAt=response.UpdatedAt,
                fees=response.Doctor.Fees,
                mode=response.AppointmentType
            });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await appointmentService.GetUserById(id);
            if (response == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error fetching pre-filled edit data"
                });
            }
            return Json(new
            {
                success=true,
                message="Data fetched",
                doctorId = response.Doctor.DoctorId,
                patientId = response.Patient.PatientId,
                doctorName= response.Doctor.User.FullName,
                patientName= response.Patient.User.FullName,
                specialization=response.Doctor.Specialization,
                date = response.Date,
                status = response.Status,
                notes = response.Notes,
                appointmentId=response.AppointmentId
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] EditAppointmentViewModel appointmentObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            if (appointmentObj.Status.HasValue)
            {
                appointmentObj.Status = (AppointmentStatusEnum)appointmentObj.Status.Value;
            }
            var response = await appointmentService.Update(appointmentObj);
            if (response != null)
            {
                return Json(new
                {
                    success = true,
                    message = "Appointment updated successfully",
                });
            }
            return Json(new { success = false, message = "Failed to update appointment" });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await appointmentService.DeleteUser(id);
            if (response != null)
            {
                return Json(new { success = true, message = "Appointment deleted successfully" });
            }
            return Json(new { success = false, message = "Failed to delete Appointment" });
        }
        [HttpGet]
        public async Task<IActionResult> AppointmentCount()
        {
            var count = await appointmentService.AppointmentCount();

            return Json(new
            {
                success = true,
                message = "Fetched appointment count",
                count = count
            });
        }
        [HttpGet]
        public async Task<IActionResult> AllAppointmentCount(int id)
        {
            var response = await appointmentService.AllAppointmentCount(id);
            return Json(new
            {
                success=true,
                message="Today's Appointment Count fetched",
                todays = response[0],
                pending = response[1],
                completed = response[2],
                cancelled = response[3]
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateViewModel appointmentStatus)
        {
            if(!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Model binding failing"
                });
            }
            if(appointmentStatus.reason!=null)
            {
                var Reason = appointmentStatus.reason;
                ReasonForRejectingAppointment = Reason;
            }

            var response = await patientService.UpdateStatus(appointmentStatus.id, appointmentStatus.status);
            if (response != null)
            {
                return Json(new
                {
                    success = true,
                    message = "Appointment status updated successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to update appointment status"
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> PatientAppointmentCount(int id)
        {
            var response = await appointmentService.AllAppointmentCount(id);
            return Json(new
            {
                success = true,
                message = "Appointment Count fetched",
                confirmed = response[4],
                completed = response[5]
            });
        }
    }
}