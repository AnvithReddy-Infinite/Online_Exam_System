using FinalProject.Common;
using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using FinalProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FinalProject.Controllers
{
    [RoutePrefix("api/exams")]
    public class ExamController : ApiController
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }


        [HttpPost]
        [Route("start")]
        public IHttpActionResult StartExam(StartExamDTO dto)
        {
            try
            {
                var response = _examService.StartExam(dto);
                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Questions EndPoint
        [HttpGet]
        [Route("{examId}/questions")]
        public IHttpActionResult GetQuestions(int examId, int userId)
        {
            try
            {
                var response = _examService.GetQuestions(examId, userId);

                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(
                    new Exception("Something went wrong while fetching questions: " + ex.Message));
            }
        }

        // Submit Endpoint
        [HttpPost]
        [Route("submit")]
        public IHttpActionResult SubmitExam(SubmitExamDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = _examService.SubmitExam(dto);

                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while submitting exam: " + ex.Message));
            }
        }


        // Result Endpoint

        [HttpGet]
        [Route("{examId}/result")]
        public IHttpActionResult GetExamResult(int examId, int userId)
        {
            try
            {
                var response = _examService.GetExamResult(examId, userId);

                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error while fetching exam result: " + ex.Message));
            }
        }


    }
}
