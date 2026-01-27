using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.User
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}