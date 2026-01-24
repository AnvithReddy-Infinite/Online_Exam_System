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
        private readonly IResultRepository _resultRepository;
        private readonly ITechRepository _techRepository;

        public ExamController()
        {
            var context = new OnlineExamSystemEntities();

            _examRepository = new ExamRepository(context);
            _questionsRepository = new QuestionRepository(context);
            _optionsRepository = new OptionRepository(context);
            _userRepository = new UserRepository(context);
            _resultRepository = new ResultRepository(context);
            _techRepository = new TechRepository(context);
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

        // Submit Endpoint
        [HttpPost]
        [Route("submit")]
        public IHttpActionResult SubmitExam(SubmitExamDTO dto)
        {
            try
            {
                var exam = _examRepository.GetById(dto.ExamId);

                if (exam == null)
                    return BadRequest("Exam not found");

                if (exam.UserId != dto.UserId)
                    return BadRequest("Unauthorized Exam Submission");

                if (exam.CompletedAt != null)
                    return BadRequest("Exam already submitted");

                // ===== Time Limit =====
                int allowedTimeMinutes;
                switch (exam.LevelId)
                {
                    case 1:
                        allowedTimeMinutes = 30; // Beginner
                        break;
                    case 2:
                        allowedTimeMinutes = 45; // Intermediate
                        break;
                    case 3:
                        allowedTimeMinutes = 60; // Advanced
                        break;
                    default:
                        allowedTimeMinutes = 30;
                        break;
                }

                TimeSpan elapsedTime = DateTime.Now - exam.StartedAt;
                bool isTimeUp = elapsedTime.TotalMinutes > allowedTimeMinutes;

                int score = 0;

                // ===== Process answers only if time is not fully expired =====
                if (!isTimeUp)
                {
                    foreach (var answer in dto.Answers)
                    {
                        var option = _optionsRepository.GetById(answer.SelectedOptionId);

                        if (option == null)
                            return BadRequest("Invalid option selected");

                        bool isCorrect = option.IsCorrect;
                        if (isCorrect)
                            score++;

                        var result = new Result
                        {
                            ExamId = exam.ExamId,
                            QuestionId = answer.QuestionId,
                            SelectedOptionId = answer.SelectedOptionId,
                            IsCorrect = isCorrect
                        };

                        _resultRepository.Add(result);
                    }
                }

                var level = exam.Level;
                var user = exam.User;
                var tech = _techRepository.GetById(exam.TechId);

                bool isPassed = !isTimeUp && score >= level.PassMarks;

                // ===== Update exam record =====
                exam.Score = score;
                exam.Status = isPassed;
                exam.CompletedAt = isTimeUp
                    ? exam.StartedAt.AddMinutes(allowedTimeMinutes)  // strict time limit
                    : DateTime.Now;

                _examRepository.Update(exam);
                _resultRepository.SaveChanges();
                _examRepository.SaveChanges();

                // ===== Response =====
                return Ok(new ApiResponse<object>
                {
                    Success = !isTimeUp,
                    Message = isTimeUp
                        ? "Time is up! Exam automatically submitted as failed."
                        : "Exam submitted successfully",
                    Data = new
                    {
                        UserName = user.FullName,
                        Technology = tech?.Name ?? "Unknown",
                        Level = level.LevelName,
                        Score = score,
                        TimeTakenMinutes = (int)Math.Min(elapsedTime.TotalMinutes, allowedTimeMinutes),
                        Result = isPassed ? "Pass" : "Fail"
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(
                    new Exception("Error while submitting exam: " + ex.Message));
            }
        }


        // Result Endpoint

        [HttpGet]
        [Route("{examId}/result")]
        public IHttpActionResult GetExamResult(int examId, int userId)
        {
            try
            {
                var exam = _examRepository.GetById(examId);

                if (exam == null)
                    return BadRequest("Exam not found");

                if (exam.UserId != userId)
                    return BadRequest("Unauthorized access");

                if (exam.CompletedAt == null)
                    return BadRequest("Exam not yet completed");

                var tech = _techRepository.GetById(exam.TechId);
                var results = _resultRepository.GetByExamId(examId);

                var response = new ExamResultDTO
                {
                    ExamId = exam.ExamId,
                    UserName = exam.User.FullName,
                    Technology = tech?.Name ?? "Unknown",
                    Level = exam.Level.LevelName,
                    Score = exam.Score,
                    PassMarks = exam.Level.PassMarks,
                    Result = exam.Status ? "Pass" : "Fail",
                    StartedAt = exam.StartedAt,
                    CompletedAt = exam.CompletedAt,
                    Questions = results.Select(r => new QuestionResultDTO
                    {
                        QuestionId = r.QuestionId,
                        QuestionText = r.Question.QuestionText,
                        SelectedOption = r.Option.OptionText,
                        CorrectOption = r.Question.Options
                                            .First(o => o.IsCorrect)
                                            .OptionText,
                        IsCorrect = r.IsCorrect
                    }).ToList()
                };

                return Ok(new ApiResponse<ExamResultDTO>
                {
                    Success = true,
                    Message = "Exam result fetched successfully",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(
                    new Exception("Error while fetching exam result: " + ex.Message));
            }
        }

    }
}
