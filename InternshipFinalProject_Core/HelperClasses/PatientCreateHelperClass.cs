using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class PatientCreateHelperClass
    {
        public string? FullName { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.")]
        public string? Password { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }

        public string? Address { get; set; } = "No Address Added";
    }
}
