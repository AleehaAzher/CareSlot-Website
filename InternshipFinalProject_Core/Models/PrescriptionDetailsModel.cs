using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class PrescriptionDetailsModel
    {
        [Key]
        public int PrescriptionDetailId {  get; set; }
        public string? MedicineName { get;  set; }
        public string? Dosage { get; set; }
        public string? Form { get; set; }
        public string? Duration { get; set; }
        public PrescriptionModel Prescription { get; set; }
        public int PrescriptionId {  get; set; }
    }
}
