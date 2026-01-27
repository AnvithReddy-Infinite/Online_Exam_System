using FinalProject.Models.DTOs;
using OES_WepApi.Helpers;
using OES_WepApi.Repository;
using OES_WepApi.Repository.Implementations;
using OES_WepApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OES_WepApi.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private readonly IAdminRepository repo;

        public AdminController()
        {
            repo = new AdminRepository();
        }

        // LOGIN
        [HttpPost]
        [Route("admin-login")]
        public IHttpActionResult Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return BadRequest("Email and Password are required");

                var admin = repo.Login(email, password);

                if (admin == null)
                    return Ok("Invalid email or password");

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while logging in: " + ex.Message));
            }
        }

        // ADD QUESTIONS (UPLOAD FILE)
        [HttpPost]
        [Route("upload-questions")]
        [SwaggerFileUpload]
        public IHttpActionResult UploadQuestions([FromUri] int techId,[FromUri] int levelId)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count == 0)
                    return BadRequest("CSV file is required");

                var file = httpRequest.Files[0];

                if (file == null || file.ContentLength == 0)
                    return BadRequest("Uploaded file is empty");

                var result = repo.UploadQuestionsFile(file, techId, levelId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // CSV / validation related issues
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                // CSV parsing issues
                return BadRequest("Invalid CSV format: " + ex.Message);
            }
            catch (Exception ex)
            {
                // System-level failures
                return InternalServerError(ex);
            }
        }


        // REMOVE QUESTIONS (DELETE FILE)
        [HttpPost]
        [Route("remove-questions-file")]
        [SwaggerFileUpload]
        public IHttpActionResult RemoveQuestionsFile(int techId, int levelId)
        {
            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count == 0)
                    return BadRequest("Please upload a CSV file.");

                var file = request.Files[0];

                if (file == null || file.ContentLength == 0)
                    return BadRequest("Please upload a valid CSV file.");

                if (!file.FileName.EndsWith(".csv"))
                    return BadRequest("Only CSV files are allowed.");

                var result = repo.RemoveQuestionsByFile(file, techId, levelId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // Validation / input related issues
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Unexpected system error
                return InternalServerError(ex);
            }
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

        [HttpPost]
        [Route("admin-reset-password")]
        public IHttpActionResult ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (model == null ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Email and new password are required");
            }

            var admin = repo.GetByEmail(model.Email);

            if (admin == null)
                return BadRequest("Admin not found");

            repo.UpdatePassword(admin, model.NewPassword);

            return Ok("Admin password reset successfully");
        }

    }
}
