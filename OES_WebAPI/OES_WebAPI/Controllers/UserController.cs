using FinalProject.Common;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using FinalProject.Services.Implementations;
using FinalProject.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] RegisterUserDTO model)
        {
            try
            {
                var response = _userService.RegisterUser(model);
                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while registering user: " + ex.Message));
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginUserDTO model)
        {
            try
            {
                var response = _userService.LoginUser(model);
                if (!response.Success)
                    return Unauthorized(); // login fails

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while logging in: " + ex.Message));
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public IHttpActionResult ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                var response = _userService.ResetPassword(model);
                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while resetting password: " + ex.Message));
            }
        }
    }
}