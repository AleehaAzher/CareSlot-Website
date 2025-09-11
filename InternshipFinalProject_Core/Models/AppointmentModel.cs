using InternshipFinalProject_Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class AppointmentModel
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime? Date { get; set; }
        public AppointmentStatusEnum? Status { get; set; } = AppointmentStatusEnum.Pending;
        public string? Notes { get; set; } = "No notes added yet";
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public string? AppointmentType { get; set; } = "Audio Call";
        //public string? AppointmentToken { get; set; }
        public int? DoctorId { get; set; }
        public DoctorModel Doctor { get; set; }
        public int? PatientId { get; set; }
        public PatientModel Patient { get; set; }
        
    }
}
