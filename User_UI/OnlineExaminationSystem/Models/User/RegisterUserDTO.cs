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
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(100, ErrorMessage = "Qualification cannot exceed 100 characters")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Year of Completion is required")]
        [Range(1900, 2100, ErrorMessage = "Enter a valid year")]
        public int YearOfCompletion { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }


    }
}