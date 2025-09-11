using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.HelperClasses
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }
    }
}
