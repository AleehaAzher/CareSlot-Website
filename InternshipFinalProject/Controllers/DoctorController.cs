using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Application.Services;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace InternshipFinalProject.Controllers
{
    public class DoctorController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IDoctorService doctorService;
        private readonly IAppointmentService appointmentService;
        private readonly IPrescriptionService prescriptionService;

        public DoctorController(IWebHostEnvironment webHostEnvironment,IDoctorService doctorService,IAppointmentService appointmentService,IPrescriptionService prescriptionService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.doctorService = doctorService;
            this.appointmentService = appointmentService;
            this.prescriptionService = prescriptionService;
        }
        public async Task<IActionResult> Status()
        {
            var id = GetCurrentUserId();
            var response = await doctorService.GetUserByIdOrEmail(int.Parse(id));
            bool profileStatus = await IsDoctorProfileComplete();
            if (profileStatus == false)
            {
                return RedirectToAction("profileHandling", "Navbar");
            }
            if (response.ApprovalStatus == "Pending")
            {
                return RedirectToAction("PendingStatus");
            }
            if (response.ApprovalStatus == "Approved")
            {
                return RedirectToAction("ApprovedStatus");
            }
            if (response.ApprovalStatus == "Rejected")
            {
                return RedirectToAction("RejectedStatus");
            }
            return View();
        }
        public IActionResult pendingStatus()
        {
            return View();
        }
        public async Task<IActionResult> ApprovedStatus()
        {
            ViewBag.name = GetCurrentUserName();
            var id=  GetCurrentUserId();

            ViewBag.id =id;
            var appointmentList = new List<AppointmentModel>();
            appointmentList = await appointmentService.todaysAppointmentList(int.Parse(id));
            return View(appointmentList);
        }
        public IActionResult RejectedStatus()
        {
            return View();
        }
        private async Task<bool> IsDoctorProfileComplete()
        {
            try
            {
                var userId = GetCurrentUserId();
                var response = await doctorService.GetUserByIdOrEmail(int.Parse(userId));

                if (response==null)
                    return false;
                if (string.IsNullOrEmpty((string)response.Qualifications)) return false;
                if (string.IsNullOrEmpty((string)response.Specialization)) return false;
                if (response.ExperienceYears == null) return false;
                if (response.Fees == null) return false;
                if (response.AvailableDays == null || !response.AvailableDays.Any()) return false;
                if (string.IsNullOrEmpty((string)response.Contact)) return false;
                if (string.IsNullOrEmpty((string)response.Gender)) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IActionResult> ManageDoctors()
        {
            var doctorListbyPending = await doctorService.GetByPendingStatus();
            var doctorList = await doctorService.GetAll();
            ViewBag.allDoctors = doctorList;
            return View(doctorListbyPending);
        }
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int id)
        {
            var response = await doctorService.GetUserByIdOrEmail(id);
            if (response == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error loading details of doctor"
                });
            }
            return Json(new
            {
                success = true,
                message = "Success loading details of doctor",
                fullname=response.User.FullName,
                email=response.User.Email,
                specialization=response.Specialization,
                qualification=response.Qualifications,
                experienceYears=response.ExperienceYears,
                fees=response.Fees,
                contact=response.Contact,
                gender=response.Gender,
                img=response.img
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateStatusRequest request)
        {
            
                var response = await doctorService.UpdateStatus(request.Id,request.Status);
            if (response!=null)
            {
                return Json(new
                {
                    success = true,
                    message = "Doctor status updated successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to update doctor status"
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> DoctorCount()
        {
            var count = await doctorService.DoctorCount();
           
            return Json(new
            {
                success = true,
                message = "Fetched doctor count",
                count = count
            });
        }
        [HttpPost]
        public async Task<ActionResult> ApplyChanges([FromBody] DoctorApplyChanges doctorObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            
            var response = await doctorService.ApplyChanges(doctorObj);
            List<DoctorModel> doctorlist = new List<DoctorModel>();

            if (response != null)
            {
                doctorlist = response;
            }
            //ViewBag.allDoctors = doctorlist;
            return PartialView("DoctorListPartialView", doctorlist);
        }
        public async Task<IActionResult> Requests()
        {
            var id = GetCurrentUserId();
            ViewBag.id = id;
            var response = await appointmentService.getAppointmentRequests(int.Parse(id));
            var list = new List<AppointmentModel>();
            if(response!=null)
            {
                list = response;
            }
            //var count = 0;
            //foreach(var item in list)
            //{
            //    count++;
            //}
            //ViewBag.count = count;
            //var list = new List<AppointmentModel>();
            //var response=await appointmentService.get
            return View(list);
        }
        public async Task<IActionResult>Appointments()
        {
            var userid = GetCurrentUserId();
            ViewBag.userid = int.Parse(userid);
            var list = new List<AppointmentModel>();
            var response = await appointmentService.GetListByDoctor(int.Parse(userid));
            if (response != null)
            {
                list = response;
            }
            return View(list);
        }
        public async Task<IActionResult> getAppointmentRequestCount()
        {
            var id = GetCurrentUserId();
            var response = await appointmentService.getAppointmentRequestCount(int.Parse(id));
            return Json(new
            {
                count=response
            });
        }
        [HttpPost]
        public async Task<IActionResult> DoctorAppointmentsApplyChanges([FromBody] PatientMyAppointmentsApplyChanges filterObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            if (filterObj.Status.HasValue)
            {
                filterObj.Status = (AppointmentStatusEnum)filterObj.Status.Value;
            }
            var userid = GetCurrentUserId();
            ViewBag.userid = int.Parse(userid);
            var response = await appointmentService.DoctorAppointmentsApplyChanges(filterObj,int.Parse( userid));
            List<AppointmentModel> appointmentlist = new List<AppointmentModel>();

            if (response != null)
            {
                appointmentlist = response;
            }
            return PartialView("DoctorAppointmentListPartialView", appointmentlist);
        }
        public async Task<IActionResult>DoctorPrescriptions()
        {
            var userid = int.Parse(GetCurrentUserId());
            var response = await prescriptionService.GetAllPrescriptions(userid, "Doctor");
            var prescriptionList = new List<PrescriptionModel>();

            if (response != null)
            {
                prescriptionList = response;
            }
            return View(prescriptionList);
        }
    }
}
