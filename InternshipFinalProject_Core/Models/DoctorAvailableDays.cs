using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class DoctorAvailableDays
    {
        [Key]
        public int? AvailabledaysId { get; set; }
        public DoctorModel Doctor { get; set; }
        public int? DoctorId { get; set; }
        public string? Day {  get; set; }
    }
}
