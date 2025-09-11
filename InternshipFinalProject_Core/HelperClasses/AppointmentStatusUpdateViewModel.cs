using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class AppointmentStatusUpdateViewModel
    {
        public int id { get; set; }
        public string? reason { get; set; }
        public string? status { get; set; }
    }
}
