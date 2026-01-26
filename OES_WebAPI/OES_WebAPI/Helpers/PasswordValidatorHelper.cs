using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Helpers
{
    public class PasswordValidatorHelper
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password is required.";
                return false;
            }

            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Password must contain at least one number.";
                return false;
            }

            if (!password.Any(ch => "!@#$%^&*()_+-={}[]:;'<>,.?/|\\~`".Contains(ch)))
            {
                errorMessage = "Password must contain at least one special character.";
                return false;
            }

            return true;
        }
        }
}