using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Application.Services;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        private readonly IAppointmentService appointmentService;
        private readonly IPrescriptionService prescriptionService;

        public PatientController(IPatientService patientService,IDoctorService doctorService,IAppointmentService appointmentService,IPrescriptionService prescriptionService)
        {
            this.patientService = patientService;
            this.doctorService = doctorService;
            this.appointmentService = appointmentService;
            this.prescriptionService = prescriptionService;
        }
        public async Task<IActionResult> ManagePatients()
        {
            var response = await patientService.GetAll();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateHelperClass managePatientObj)
        {
            var response = await patientService.CreateUser(managePatientObj);
            Console.WriteLine(response);
            if (response.Success == true)
            {

                return Json(new
                {
                    success = true,
                    message = "User created successfully",
                });
            }
            return Json(new { success = false, message = "Failed to create user" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await patientService.GetUserByIdOrEmail(id);
            if (response != null)
            {
                return Json(new
                {
                    success = true,
                    userId = response.UserId,
                    email = response.User.Email,
                    fullName = response.User.FullName,
                    contact = response.Contact,
                    gender = response.Gender,
                    age = response.Age,
                    bloodGroup = response.BloodGroup,
                    address = response.Address,
                    gName = response.EmergencyContactName,
                    gContact = response.EmergencyContact,
                    patientId = response.PatientId
                });
            }
            return Json(new { success = false, message = "User not found" });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditPatient patient)
        {

            var response = await patientService.UpdateUser(patient);
            if (response.Success == true)
            {
                return Json(new
                {
                    success = true,
                    message = response.Message
                });
            }
            return Json(new { success = false, message = "Failed to update user" });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await patientService.DeleteUser(id);
            if (response != null)
            {
                return Json(new { success = true, message = "User deleted successfully" });
            }
            return Json(new { success = false, message = "Failed to delete user" });
        }
        [HttpGet]
        public async Task<IActionResult> PatientCount()
        {
            var count = await patientService.PatientCount();

            return Json(new
            {
                success = true,
                message = "Fetched patient count",
                count = count
            });
        }
        public async Task<IActionResult> profileStatus()
        {
            var userId = GetCurrentUserId();
            bool profileStatus = await IsPatientProfileComplete();
            if (profileStatus == false)
            {
                return RedirectToAction("profileHandling", "Navbar");
            }
            else
            {
                return RedirectToAction("PatientDashboard", "Home");
            }
        }
        private async Task<bool> IsPatientProfileComplete()
        {
            try
            {
                var userId = GetCurrentUserId();
                var response = await patientService.GetUserByIdOrEmail(int.Parse(userId));
                if (response == null)
                    return false;
                if (response.Age == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.BloodGroup))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.Contact))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.Gender))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.EmergencyContactName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.EmergencyContact))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.Address))
                {
                    return false;
                }
                if (string.IsNullOrEmpty((string)response.img))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public  async Task<IActionResult> BookAppointments()
        {
            var userid = GetCurrentUserId();
            var list = new List<DoctorModel>();
            var response = await doctorService.GetAll();
            if (response != null)
            {
                list = response;
            }
            ViewBag.userid = userid;
            return View(list);
        }
        public async Task<IActionResult> MyAppointments()
        {
            var userid = GetCurrentUserId();
            ViewBag.userid = userid;
            var list = new List<AppointmentModel>();
            var response = await appointmentService.GetListByPatient(int.Parse(userid));
            if (response != null)
            {
                list = response;
            }
            return View(list);
        }
        public IActionResult Favorites()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BookAppointmentApplyFilters([FromBody] BookAppointmentApplyFilters filterObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }

            var response = await doctorService.BookAppointmentApplyFilters(filterObj);
            List<DoctorModel> doctorlist = new List<DoctorModel>();

            if (response != null)
            {
                doctorlist = response;
            }
            return PartialView("BookAppointmentListPartialView", doctorlist);
        }
        public async Task<IActionResult> getMyAppointmentCount()
        {
            var id = GetCurrentUserId();
            var response = await appointmentService.getMyAppointmentCount(int.Parse(id));
            return Json(new
            {
                count = response
            });
        }
        public async Task<IActionResult> PatientPrescriptions()
        {
            var userid = int.Parse(GetCurrentUserId());
            var response = await prescriptionService.GetAllPrescriptions(userid,"Patient");
            var prescriptionList = new List<PrescriptionModel>();

            if (response!=null)
            {
                prescriptionList = response;
            }
            return View(prescriptionList);
        }
        [HttpPost]
        public async Task<IActionResult> PatientMyAppointmentsApplyChanges([FromBody] PatientMyAppointmentsApplyChanges filterObj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data received" });
            }
            if (filterObj.Status.HasValue)
            {
                filterObj.Status = (AppointmentStatusEnum)filterObj.Status.Value;
            }
            var userid = int.Parse(GetCurrentUserId());
            var response = await appointmentService.PatientMyAppointmentsApplyChanges(filterObj,userid);
            List<AppointmentModel> appointmentlist = new List<AppointmentModel>();

            if (response != null)
            {
                appointmentlist = response;
            }
            return PartialView("MyAppointmentListPartialView", appointmentlist);
        }
        //public IActionResult GenerateInvoice()
        //{
        //    return View();
        //}
    }
}