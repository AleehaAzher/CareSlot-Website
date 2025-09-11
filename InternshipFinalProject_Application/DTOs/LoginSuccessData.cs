using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.DTOs
{
    public class LoginSuccessData
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }


    }
}
