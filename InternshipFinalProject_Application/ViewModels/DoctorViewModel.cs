using InternshipFinalProject_Core.HelperClasses;
using InternshipFinalProject_Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.ViewModels
{
    public class DoctorViewModel
    {
        public int id { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? fullName { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? Qualifications { get; set; }

        public string? Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public List<string>? AvailableDays { get; set; }
        public DateTime? AvailableFromTime { get; set; }

        public DateTime? AvailableToTime { get; set; }

        public float? Fees { get; set; }
        public string? Contact { get; set; }
        public string? Gender { get; set; }
        public string? Bio { get; set; }
    }
}
