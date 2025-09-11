using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class EditAppointmentViewModel
    {
        public DateTime? Date { get; set; }
        public AppointmentStatusEnum? Status { get; set; } = AppointmentStatusEnum.Pending;
        public string? Notes { get; set; } = "No notes added yet";
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentId {  get; set; }
        public string? AppointmentType { get; set; }
    }
}
