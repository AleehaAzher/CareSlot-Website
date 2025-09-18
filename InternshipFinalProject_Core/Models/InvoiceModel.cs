using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class InvoiceModel
    {
            [Key]
            public int InvoiceId { get; set; }

            public int? AppointmentId { get; set; }
            public AppointmentModel Appointment { get; set; }

            public int? PatientId { get; set; }
            public PatientModel Patient { get; set; }

            public int? DoctorId { get; set; }
            public DoctorModel Doctor { get; set; }

            public double? Amount { get; set;}
            public bool? PaidStatus { get; set; }
            public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
            //public DateTime DueDate { get; set; } = DateTime.UtcNow;

    }
}
