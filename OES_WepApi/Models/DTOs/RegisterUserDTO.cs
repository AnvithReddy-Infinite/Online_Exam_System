using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models.DTOs
{
    public class RegisterUserDTO
    {
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime? DOB { get; set; }
        public string Qualification { get; set; }
        public int? YearOfCompletion { get; set; }
    }
}