using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class AppointmentApplyChanges
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? Date { get; set; }
        public AppointmentStatusEnum? Status { get; set; }
    }
}
