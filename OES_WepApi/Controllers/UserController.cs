using OES_WepApi.Models;
using OES_WepApi.Repository;
using System;
using System.Linq;
using System.Web.Http;

namespace OES_WepApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepo;

        public UserController()
        {
            _userRepo = new UserRepository();
        }

        // Register User in DB
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] User user)
        {
            // Condition for required fields
            if (user == null || string.IsNullOrEmpty(user.FullName) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest("FullName, Email, and Password are required");
            }

            // Check if email already exists
            if (_userRepo.GetUserByEmail(user.Email) != null)
                return BadRequest("Email already exists");

            // Hashing password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            user.CreatedAt = DateTime.Now;

            // Add user to DB
            _userRepo.AddUser(user);
            return Ok("Registration successful");
        }


        // Login Handler
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email and Password are required");
            }

            var user = _userRepo.GetUserByEmail(model.Email);

            if (user == null || !_userRepo.ValidateUser(model.Email, model.Password))
            {
                return Unauthorized(); // Either user not found or password mismatch
            }

            // Return user info (exclude password)
            return Ok("Login Successfull");
        }

        // For admin checking
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepo.GetAllUsers();

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
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // Login Model
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
