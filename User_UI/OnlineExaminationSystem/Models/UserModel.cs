using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace OnlineExaminationSystem.Models
{
    public class UserModel
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
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10 digit mobile number")]

       
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        public string Qualification { get; set; }
        public int? YearOfCompletion { get; set; }
        public DateTime CreatedAt { get; set; }
 
    }
}