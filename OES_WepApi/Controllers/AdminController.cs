using OES_WepApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OES_WepApi.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdminRepository repo = new AdminRepository();

        [HttpPost]
        [Route("admin-login")]
        public IHttpActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest("Email and Password are required");

            var admin = repo.Login(email, password);

            if (admin == null)
                return Ok("Invalid email or password");

            return Ok(admin);
        }

        [HttpGet]
        [Route("search-students")]
        public IHttpActionResult SearchStudents(int? techId = null, int? levelId = null, string state = null,
        string city = null, int? minMarks = null)
        {
            var students = repo.SearchStudents(
                techId, levelId, state, city, minMarks);

            if (students.Count == 0)
                return Ok("No students found for selected criteria");

            return Ok(students);
        }
    }
}
