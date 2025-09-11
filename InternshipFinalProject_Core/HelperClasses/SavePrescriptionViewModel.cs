using InternshipFinalProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class SavePrescriptionViewModel
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public string Diagnosis { get; set; }
        public string Advice { get; set; }

        public List<SavePrescriptionDetailsViewModel> PrescriptionDetailsColumn { get; set; } = new List<SavePrescriptionDetailsViewModel>();
    }
}
