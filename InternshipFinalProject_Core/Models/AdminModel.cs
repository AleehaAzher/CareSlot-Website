using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core.Models
{
    public class AdminModel
    {
        [Key]
        public int AdminId { get; set; }
        public string? img { get; set; }
        public int UserId {  get; set; }
        public UserModel User { get; set; }
    }
}
