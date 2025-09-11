using InternshipFinalProject_Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class DoctorModel
    {
        [Key]
        public int DoctorId { get; set; }
        public string? Qualifications { get; set; }

        public string? Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public ICollection<DoctorAvailableDays>? AvailableDays { get; set; } = new List<DoctorAvailableDays>();
        public DateTime? AvailableFromTime { get; set; }

        public DateTime? AvailableToTime { get; set; }

        public float? Fees { get; set; }
        public string? Contact { get; set; }
        public string? Gender { get; set; }
        public string? Bio { get; set; } = "No Bio Added";
        public string ApprovalStatus { get; set; } = "Pending";

        public string? img { get; set; }
        public int UserId { get; set; }
        //[JsonIgnore]
        public UserModel User { get; set; }
        //[JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();
        public List<PrescriptionModel>? Prescriptions { get; set; } = new();
    }
}
