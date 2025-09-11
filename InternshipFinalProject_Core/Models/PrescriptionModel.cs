using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class PrescriptionModel
    {
        [Key]
        public int PrescriptionId { get; set; }
        public DateTime? PrescribedDate {  get; set; }
        public string? Diagnosis { get; set; }
        public string? Advice {  get; set; }

        public DoctorModel Doctor { get; set; }
        public int? DoctorId { get; set; }
        public PatientModel Patient { get; set; }
        public int? PatientId { get; set; }
        public List<PrescriptionDetailsModel> PrescriptionDetails { get; set; } = new();
    }
}