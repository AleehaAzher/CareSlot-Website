using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class PatientDTO
    {
        public int id { get; set; }
        public string? fullName { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public int? Age { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }

        public string? Address { get; set; } = "No Address Added";
        public string? img { get; set; }
    }
}
