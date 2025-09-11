using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Application.Services;
using InternshipFinalProject_Application.ViewModels;
using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace InternshipFinalProject.Controllers
{
    public class NavbarController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserService userService;
        private readonly IAdminService adminService;
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;

        public NavbarController(IWebHostEnvironment webHostEnvironment,IUserService userService,IAdminService adminService,IDoctorService doctorService,IPatientService patientService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userService = userService;
            this.adminService = adminService;
            this.doctorService = doctorService;
            this.patientService = patientService;
        }

        public async Task<IActionResult> profileHandling()
        {
            var currentRole = GetCurrentUserRole();
            if (currentRole == "Admin")
            {
                return RedirectToAction("AdminProfile");
            }
            if (currentRole == "Doctor")
            {
                bool profileComplete = await IsDoctorProfileComplete();
                if(profileComplete)
                {
                    return RedirectToAction("StatusDoctorProfile");
                }
                else
                {
                    return RedirectToAction("DoctorProfile");
                }
            }   
            if (currentRole == "Patient")
            {
                bool profileComplete = await IsPatientProfileComplete();
                if (profileComplete)
                {
                    return RedirectToAction("CompletedPatientProfile");
                }
                else
                {
                    return RedirectToAction("PatientProfile");
                }
            }
            return View();
        }

        public IActionResult AdminProfile()
        {
            return View();
        }
        public IActionResult StatusDoctorProfile()
        {
            return View();
        }
        public IActionResult DoctorProfile()
        {
            return View();
        }
        public IActionResult PatientProfile()
        {
            return View();
        }
        public IActionResult CompletedPatientProfile()
        {
            return View();
        }
        [HttpGet]
        public IActionResult getId()
        {
            var id = GetCurrentUserId();
            return Json(new { id });
        }

        [HttpPost]
        public async Task<IActionResult> getData(int id)
        {
            var user = await userService.GetUserByIdOrEmail(id);
            var role = GetCurrentUserRole();
            if (role == "Admin")
            {
                string imagePath = null;
                if (user!=null)
                {
                    //string data = await response.Content.ReadAsStringAsync();
                    //var user = JsonConvert.DeserializeObject<UserModel>(data);
                    var adminResponse = await adminService.GetUserByIdOrEmail(id);
                    if (adminResponse!=null)
                    {
                        
                        imagePath = adminResponse?.img;
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Data retrieved successfully.",
                        name = user.FullName,
                        email = user.Email,
                        img = imagePath
                    });
                }
            }
            if (role == "Doctor")
            {
                string imagePath = null;
                if (user!=null)
                {
                    var doctor = await doctorService.GetUserByIdOrEmail(id);
                    if (doctor!=null)
                    { 
                        imagePath = doctor?.img;


                        return Json(new
                        {
                            success = true,
                            message = "Data retrieved successfully.",
                            name = user.FullName,
                            email = user.Email,
                            img = imagePath,
                            contact = doctor.Contact,
                            qualifications = doctor.Qualifications,
                            experienceYears = doctor.ExperienceYears,
                            fees = doctor.Fees,
                            bio = doctor.Bio
                        });
                    }

                }
            }
            if (role == "Patient")
            {
                string imagePath = null;
                if (user != null)
                {
                    var patient = await patientService.GetUserByIdOrEmail(id);
                    if (patient != null)
                    {
                        imagePath = patient?.img;


                        return Json(new
                        {
                            success = true,
                            message = "Data retrieved successfully.",
                            name = user.FullName,
                            email = user.Email,
                            img = imagePath,
                            contact = patient.Contact,
                            gender = patient.Gender,
                            gName = patient.EmergencyContactName,
                            gContact = patient.EmergencyContact,
                            address = patient.Address,
                            age=patient.Age,
                            bloodGroup=patient.BloodGroup
                        });
                    }

                }
            }
            return Json(new { success = false, message = "Error retrieving data" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(AdminHelperClass model)
        {
            try
            {
                string imagePath = null;

                var imageFile = Request.Form.Files["img"];

                if (imageFile != null && imageFile.Length > 0)
                {
                    imagePath = await SaveImageToFrontend(imageFile);
                }

                var id = int.Parse(Request.Form["id"]);
                var email = Request.Form["email"];
                var password = Request.Form["password"];
                var fullName = Request.Form["fullName"];

                var apiData = new AdminDTO
                {
                    id = id,
                    img = imagePath,
                    fullName = fullName.ToString(),
                    email = email.ToString(),
                    password = password.ToString()
                };
                var updateResponse = await adminService.updateAdmin(apiData);
                

                if (updateResponse!=null)
                { 
                    return Json(new
                    {
                        success = true,
                        message = "Profile updated successfully",
                        imagePath = imagePath
                    });
                }

                return Json(new { success = false, message = "Failed to update profile" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<string> SaveImageToFrontend(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "admin-images");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/admin-images/{fileName}";
        }
        [HttpGet]
        public async Task<IActionResult> GetProfileImage()
        {
            var id = int.Parse(GetCurrentUserId());
            var role = GetCurrentUserRole();

            string imagePath = "/assets/img/avatars/defaultAvatar.jpg";

            if (role == "Admin")
            {
                var response = await adminService.GetUserByIdOrEmail(id);
                if (response!= null && response.img != null)
                {
                        imagePath = response.img;
                }
            }
            if (role == "Doctor")
            {
                var doctorResponse = await doctorService.GetUserByIdOrEmail(id);
                if (doctorResponse!=null)
                {
                    if (doctorResponse != null && doctorResponse.img != null)
                    {
                        imagePath = doctorResponse.img;
                    }
                }
            }
            if (role == "Patient")
            {
                var patientResponse= await patientService.GetUserByIdOrEmail(id);
                if (patientResponse != null)
                {
                    if (patientResponse != null && patientResponse.img != null)
                    {
                        imagePath = patientResponse.img;
                    }
                }
            }

            return Json(new { img = imagePath });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDoctorProfile(DoctorViewModel model)
        {
            try
            {
                string imagePath = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imagePath = await SaveDoctorImageToFrontend(model.ImageFile);
                }
                DateTime? availableFromTime = null;
                DateTime? availableToTime = null;

                if (DateTime.TryParse(Request.Form["AvailableFromTime"], out DateTime fromTime))
                {
                    availableFromTime = fromTime;
                }

                if (DateTime.TryParse(Request.Form["AvailableToTime"], out DateTime toTime))
                {
                    availableToTime = toTime;
                }
                //WeekdaysEnum availableDays = WeekdaysEnum.None;
                var daysList = new List<string>();
                string availableDaysString = Request.Form["AvailableDays"].ToString();

                if (!string.IsNullOrEmpty(availableDaysString))
                {
                    string[] days = availableDaysString.Split(',');
                    foreach (string day in days)
                    {

                        daysList.Add(day.Trim());
                    }
                }
                var apiData = new DoctorDTO
                {
                    id = model.id,
                    fullName = model.fullName,
                    email = model.email,
                    password = model.password,
                    Contact = model.Contact,
                    Qualifications = model.Qualifications,
                    Specialization = model.Specialization,
                    ExperienceYears = model.ExperienceYears,
                    Fees = model.Fees,
                    AvailableDays = daysList,
                    AvailableFromTime = availableFromTime,
                    AvailableToTime = availableToTime,
                    Gender = model.Gender,
                    Bio = model.Bio,
                    img = imagePath
                };

                var doctorResponse = await doctorService.updateDoctor(apiData);
                if (doctorResponse!=null)
                {
                    return Json(new { success = true, message = "Profile updated successfully" });
                }

                return Json(new { success = false, message = "Failed to update profile" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        private async Task<string> SaveDoctorImageToFrontend(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "doctor-images");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/doctor-images/{fileName}";
        }
        private async Task<bool> IsDoctorProfileComplete()
        {
            try
            {
                var userId = GetCurrentUserId();
                var response = await doctorService.GetUserByIdOrEmail(int.Parse(userId));

                if (response == null)
                    return false;
                if (string.IsNullOrEmpty((string)response.Qualifications)) return false;
                if (string.IsNullOrEmpty((string)response.Specialization)) return false;
                if (response.ExperienceYears == null) return false;
                if (response.Fees == null) return false;
                if (string.IsNullOrEmpty((string)response.Contact)) return false;
                if (response.AvailableDays == null || !response.AvailableDays.Any()) return false;
                if (string.IsNullOrEmpty((string)response.Gender)) return false;

                return true;
            }
            catch
            {
                return false;
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
                if (response.Age == null) return false;
                if (string.IsNullOrEmpty((string)response.BloodGroup)) return false;
                if (string.IsNullOrEmpty((string)response.Contact)) return false;
                if (string.IsNullOrEmpty((string)response.Gender)) return false;
                if (string.IsNullOrEmpty((string)response.EmergencyContactName)) return false;
                if (string.IsNullOrEmpty((string)response.EmergencyContact)) return false;
                if (string.IsNullOrEmpty((string)response.Address)) return false;
                if (string.IsNullOrEmpty((string)response.img)) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePatientProfile(PatientViewModel model)
        {
            try
            {
                string imagePath = null;

                if (model.img != null && model.img.Length > 0)
                {
                    imagePath = await SavePatientImageToFrontend(model.img);
                }
               
                var apiData = new PatientDTO
                {
                    id = model.id,
                    fullName = model.fullName,
                    email = model.email,
                    password = model.password,
                    Contact = model.Contact,
                    Age = model.Age,
                    EmergencyContact = model.EmergencyContact,
                    EmergencyContactName = model.EmergencyContactName,
                    Gender = model.Gender,
                    BloodGroup = model.BloodGroup,
                   
                    Address = model.Address,
                    img = imagePath
                };

                var patientResponse = await patientService.updatePatient(apiData);
                if (patientResponse != null)
                {
                    return Json(new { success = true, message = "Profile updated successfully" });
                }

                return Json(new { success = false, message = "Failed to update profile" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        private async Task<string> SavePatientImageToFrontend(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "patient-images");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/patient-images/{fileName}";
        }
    }
}
