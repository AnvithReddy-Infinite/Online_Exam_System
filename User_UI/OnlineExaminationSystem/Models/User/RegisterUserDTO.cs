using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.User
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login ID is required is required")]
        [StringLength(50, ErrorMessage = "Login ID cannot be longer than 50 characters")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        //[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10 digit mobile number")]


        public string Mobile { get; set; }
        [Required(ErrorMessage = "City is required")]

        public string City { get; set; }
        [Required(ErrorMessage = "City is required")]

        public string State { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        [Required(ErrorMessage = "Qualification is required")]

        public string Qualification { get; set; }
        [Required(ErrorMessage = "YearOfCompletion is required")]

        public int? YearOfCompletion { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Captcha { get; set; }


    }
}