using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.User
{
    public class RegisterUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DOB { get; set; }
        public string Qualification { get; set; }
        public int YearOfCompletion { get; set; }
        public string Password { get; set; }
    }
}