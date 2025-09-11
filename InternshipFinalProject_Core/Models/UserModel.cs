using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.")]

        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } 

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ResetToken { get; set; }     
        public DateTime? TokenExpiry { get; set; }
        public DoctorModel? Doctor { get; set; }
        public PatientModel? Patient { get; set; }

    }
}
