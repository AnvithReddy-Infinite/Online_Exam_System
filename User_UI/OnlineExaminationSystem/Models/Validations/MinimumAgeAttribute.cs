using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem.Models.Validations
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date of Birth is required");

            DateTime dob = (DateTime)value;
            int age = DateTime.Today.Year - dob.Year;

            if (dob > DateTime.Today.AddYears(-age))
                age--;

            if (age < _minimumAge)
                return new ValidationResult($"User must be at least {_minimumAge} years old");

            return ValidationResult.Success;
        }
    }
}
