using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required, EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.")]
        public string NewPassword { get; set; }
    }
}
