using FinalProject.Common;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Interfaces;
using FinalProject.Services.Interfaces;
using OES_WebAPI.Models;
using System;


namespace FinalProject.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Register service
        public ApiResponse<object> RegisterUser(RegisterUserDTO model)
        {
            // Validate email
            if (!EmailValidatorHelper.IsValidEmail(model.Email))
                return new ApiResponse<object>(false, "Invalid email format", null);

            // Validate Password
            if (!PasswordValidatorHelper.IsValid(model.Password, out string passwordError))
                return new ApiResponse<object>(false, passwordError, null);

            // Check user existence
            var existingUser = _userRepository.GetByEmail(model.Email);
            if (existingUser != null)
                return new ApiResponse<object>(false, "Email already registered", null);

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Mobile = model.Mobile,
                City = model.City,
                State = model.State,
                DOB = model.DOB,
                Qualification = model.Qualification,
                YearOfCompletion = model.YearOfCompletion,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                CreatedAt = DateTime.Now
            };

            _userRepository.AddUser(user);
            _userRepository.SaveChanges();

            return new ApiResponse<object>(true, "User registered successfully", new
            {
                user.UserId,
                user.FullName,
                user.Email
            });
        }

        // Login Service
        public ApiResponse<object> LoginUser(LoginUserDTO model)
        {
            var user = _userRepository.GetByEmail(model.Email);

            // Check user existence
            if (user == null)
                return new ApiResponse<object>(false, "Invalid email or password", null);

            // Validate Password
            bool isPasswordValid = PasswordHelper.VerifyPassword(model.Password, user.PasswordHash);
            if (!isPasswordValid)
                return new ApiResponse<object>(false, "Invalid email or password", null);

            // Response
            return new ApiResponse<object>(true, "Login successful", new
            {
                user.UserId,
                user.FullName,
                user.Email
            });
        }

        // Reset Password Service
        public ApiResponse<string> ResetPassword(ResetPasswordDTO model)
        {
            var user = _userRepository.GetByEmail(model.Email);
            if (user == null)
                return new ApiResponse<string>(false, "User not found", null);

            if (!PasswordValidatorHelper.IsValid(model.NewPassword, out string passwordError))
                return new ApiResponse<string>(false, passwordError, null);

            user.PasswordHash = PasswordHelper.HashPassword(model.NewPassword);
            _userRepository.SaveChanges();

            return new ApiResponse<string>(true, "Password updated successfully", null);
        }

    }
}
