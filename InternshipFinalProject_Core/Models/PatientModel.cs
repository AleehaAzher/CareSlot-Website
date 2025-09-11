using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class PatientModel
    {
        [Key]
        public int PatientId { get; set; }
        public int? Age { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }

        public string? Address { get; set; } = "No Address Added";
        public string? img { get; set; }

        public int UserId { get; set; }
        //[JsonIgnore]
        public UserModel User { get; set; }
        //[JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();
        public List<PrescriptionModel>? Prescriptions { get; set; } = new();

    }
}
