using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using FinalProject.Common;
using FinalProject.Helpers;
using System;
using System.Linq;
using System.Web.Http;
using OES_WepApi.Models;

namespace FinalProject.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;

        // Initialize repository directly
        public UserController()
        {
            var context = new OnlineExamSystemEntities(); // EF DbContext
            _userRepository = new UserRepository(context);
        }

        // Register EndPoint
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] RegisterUserDTO model)
        {
            if (model == null)
                return BadRequest("User data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Validate Email
                if (!EmailValidatorHelper.IsValidEmail(model.Email))
                {
                    return BadRequest("Invalid email format.");
                }

                // Validate Password
                if (!PasswordValidatorHelper.IsValid(model.Password, out string passwordError))
                    return BadRequest(passwordError);

                // Check if email already exists
                var existingUser = _userRepository.GetByEmail(model.Email);
                if (existingUser != null)
                    return Conflict(); // 409 Conflict

                // Map DTO to User model
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

                // Save user
                _userRepository.AddUser(user);
                _userRepository.SaveChanges();

                // Return response
                return Ok(new ApiResponse<object>(
                    true,
                    "User registered successfully.",
                    new
                    {
                        user.UserId,
                        user.FullName,
                        user.Email
                    }
                ));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return InternalServerError(ex);
            }
        }

        // Login EndPoint

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginUserDTO model)
        {
            if (model == null)
                return BadRequest("Login data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get user by email
                var user = _userRepository.GetByEmail(model.Email);
                if (user == null)
                    return Unauthorized(); // email doesn't exist

                // Verify password
                bool isPasswordValid = PasswordHelper.VerifyPassword(model.Password, user.PasswordHash);
                if (!isPasswordValid)
                    return Unauthorized(); // password is wrong

                return Ok(new ApiResponse<object>(true, "Login successful", new
                {
                    user.UserId,
                    user.FullName,
                    user.Email
                }));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return InternalServerError(ex);
            }
        }

        // Reset Password EndPoint
        [HttpPost]
        [Route("reset-password")]
        public IHttpActionResult ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (model == null)
                return BadRequest("Data is required.");

            var user = _userRepository.GetByEmail(model.Email);
            if (user == null)
                return NotFound();

            if(!PasswordValidatorHelper.IsValid(model.NewPassword, out string passwordError))
            return BadRequest(passwordError);

            user.PasswordHash = PasswordHelper.HashPassword(model.NewPassword);
            _userRepository.SaveChanges();

            return Ok(new ApiResponse<string>(true, "Password updated successfully.", null));
        }


        // Users List EndPoint
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();

                if (users == null || !users.Any())
                    return Ok(new ApiResponse<object>(true, "No users found", null));

                var result = users.Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Email,
                    u.Mobile,
                    u.City,
                    u.State,
                    u.DOB,
                    u.Qualification,
                    u.YearOfCompletion,
                    u.CreatedAt
                }).ToList();

                return Ok(new ApiResponse<object>(true, "Users fetched successfully", result));
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An unexpected error occurred while fetching users.", ex));
            }
        }
    }
}
