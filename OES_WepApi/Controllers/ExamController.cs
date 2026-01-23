using FinalProject.Common;
using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using OES_WepApi.Models;
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
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionsRepository;
        private readonly IOptionRepository _optionsRepository;
        private readonly IUserRepository _userRepository;

        public ExamController()
        {
            var context = new OnlineExamSystemEntities();

            _examRepository = new ExamRepository(context);
            _questionsRepository = new QuestionRepository(context);
            _optionsRepository = new OptionRepository(context);
            _userRepository = new UserRepository(context);
        }

        [HttpPost]
        [Route("start")]
        public IHttpActionResult StartExam(StartExamDTO dto)
        {
            try
            {
                var user = _userRepository.GetById(dto.UserId);

                // Check if user exists
                if (user == null)
                    return BadRequest("Invalid user");

                // Higher level preventions
                if (dto.LevelId > 1)
                {
                    var previouLevel = _examRepository.GetByUserTechLevel(dto.UserId, dto.TechId, dto.LevelId - 1);

                    if (!previouLevel.Any(e => e.Status == true))
                        return BadRequest("You must pass the previous level before attempting this");
                }

                // In progress exam for same tech and level
                var inProgress = _examRepository.GetInProgressExams(dto.UserId, dto.TechId, dto.LevelId);

                if (inProgress.Any())
                    return BadRequest("You already have an in-progress exam for this level");

                // New Exam Record
                var exam = new Exam
                {
                    UserId = dto.UserId,
                    TechId = dto.TechId,
                    LevelId = dto.LevelId,
                    StartedAt = DateTime.Now,
                    CompletedAt = null,
                    Score = 0,
                    Status = false
                };

                _examRepository.AddExam(exam);
                _examRepository.SaveChanges();

                return Ok(new ApiResponse<StartExamResponseDTO>
                {
                    Success = true,
                    Message = "Exam started successfully",
                    Data = new StartExamResponseDTO
                    {
                        ExamId = exam.ExamId,
                        UserId = exam.UserId
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the exception in real application
                return InternalServerError(new Exception("Something went wrong while fetching questions." + ex));
            }

        }

        // Questions EndPoint
        [HttpGet]
        [Route("{examId}/questions")]
        public IHttpActionResult GetQuestions(int examId, int userId)
        {
            try
            {
                var exam = _examRepository.GetById(examId);
                if (exam == null)
                {
                    return BadRequest("Exam Not Found");
                }

                if (exam.UserId != userId)
                    return BadRequest("Unauthorized access to this exam");

                var questions = _questionsRepository.GetByTechAndLevel(exam.TechId, exam.LevelId);

                var questionDTOs = questions.Select(q => new QuestionDTO
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Options = _optionsRepository.GetByQuestionId(q.QuestionId).Select(o => new OptionDTO
                    {
                        OptionId = o.OptionId,
                        OptionText = o.OptionText
                    }).ToList()
                }).ToList();

                return Ok(new ApiResponse<List<QuestionDTO>>
                {
                    Success = true,
                    Message = "Questions retrieved successfully",
                    Data = questionDTOs
                });
            }
            catch (Exception ex)
            {
                // Log the exception in real application
                return InternalServerError(new Exception("Something went wrong while fetching questions." + ex));
            }

        }
    }
}
