using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class EditPatient
    {
        public int? Age { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; } = "Not added";
        public string? Contact { get; set; } = "Not added";
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }

        public string? Address { get; set; } = "No Address Added";

        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
