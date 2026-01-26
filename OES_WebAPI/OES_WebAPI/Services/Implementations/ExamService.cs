using FinalProject.Common;
using FinalProject.Models;
using FinalProject.Models.DTOs;
using FinalProject.Repositories.Implementations;
using FinalProject.Repositories.Interfaces;
using FinalProject.Services.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Services.Implementations
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IResultRepository _resultRepository;
        private readonly ITechRepository _techRepository;

        public ExamService(IExamRepository examRepository, IUserRepository userRepository, IQuestionRepository questionRepository, 
                           IOptionRepository optionRepository, IResultRepository resultRepository, ITechRepository techRepository)
        {
            _examRepository = examRepository;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _resultRepository = resultRepository;
            _techRepository = techRepository;
        }

        // Exam Start Register Service
        public ApiResponse<StartExamResponseDTO> StartExam(StartExamDTO dto)
        {
            // Validate user
            var user = _userRepository.GetById(dto.UserId);
            if (user == null)
            {
                return new ApiResponse<StartExamResponseDTO>
                {
                    Success = false,
                    Message = "Invalid user"
                };
            }

            // Previous level check
            if (dto.LevelId > 1)
            {
                var previousLevelExams = _examRepository
                    .GetByUserTechLevel(dto.UserId, dto.TechId, dto.LevelId - 1);

                if (!previousLevelExams.Any(e => e.Status))
                {
                    return new ApiResponse<StartExamResponseDTO>
                    {
                        Success = false,
                        Message = "You must pass the previous level before attempting this"
                    };
                }
            }

            // Check in-progress exam
            var inProgress = _examRepository
                .GetInProgressExams(dto.UserId, dto.TechId, dto.LevelId);

            if (inProgress.Any())
            {
                return new ApiResponse<StartExamResponseDTO>
                {
                    Success = false,
                    Message = "You already have an in-progress exam for this level"
                };
            }

            // Create exam
            var exam = new Exam
            {
                UserId = dto.UserId,
                TechId = dto.TechId,
                LevelId = dto.LevelId,
                StartedAt = DateTime.UtcNow,
                CompletedAt = null,
                Score = 0,
                Status = false
            };

            _examRepository.AddExam(exam);
            _examRepository.SaveChanges();

            // Response
            return new ApiResponse<StartExamResponseDTO>
            {
                Success = true,
                Message = "Exam started successfully",
                Data = new StartExamResponseDTO
                {
                    ExamId = exam.ExamId,
                    UserId = exam.UserId
                }
            };
        }

        // Questions Retrieval Service
        public ApiResponse<List<QuestionDTO>> GetQuestions(int examId, int userId)
        {
            // Fetch exam
            var exam = _examRepository.GetById(examId);
            if (exam == null)
                return new ApiResponse<List<QuestionDTO>>
                {
                    Success = false,
                    Message = "Exam not found"
                };

            if (exam.UserId != userId)
                return new ApiResponse<List<QuestionDTO>>
                {
                    Success = false,
                    Message = "Unauthorized access to this exam"
                };

            // Fetch questions
            var questions = _questionRepository.GetByTechAndLevel(exam.TechId, exam.LevelId);
            if (questions == null || !questions.Any())
                return new ApiResponse<List<QuestionDTO>>
                {
                    Success = true,
                    Message = "No questions available for this exam",
                    Data = new List<QuestionDTO>()
                };

            // Fetch all options
            var questionIds = questions.Select(q => q.QuestionId).ToList();
            var allOptions = _optionRepository.GetByQuestionIds(questionIds) ?? new List<Option>();

            // Map to DTOs
            var questionDTOs = questions.Select(q => new QuestionDTO
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                Options = allOptions.Where(o => o.QuestionId == q.QuestionId)
                            .Select(o => new OptionDTO
                            {
                                OptionId = o.OptionId,
                                OptionText = o.OptionText
                            })
                            .ToList()
            }).ToList();

            // Response
            return new ApiResponse<List<QuestionDTO>>
            {
                Success = true,
                Message = "Questions retrieved successfully",
                Data = questionDTOs
            };
        }

        // Submit Exam Service
        public ApiResponse<SubmitExamResponseDTO> SubmitExam(SubmitExamDTO dto)
        {
            try
            {
                var exam = _examRepository.GetById(dto.ExamId);
                if (exam == null)
                    return new ApiResponse<SubmitExamResponseDTO>
                    {
                        Success = false,
                        Message = "Exam not found"
                    };

                // Validate user
                if (exam.UserId != dto.UserId)
                    return new ApiResponse<SubmitExamResponseDTO>
                    {
                        Success = false,
                        Message = "Unauthorized Exam Submission"
                    };

                // Validate Submission
                if (exam.CompletedAt != null)
                    return new ApiResponse<SubmitExamResponseDTO>
                    {
                        Success = false,
                        Message = "Exam already submitted"
                    };

                // Time Limit 
                int allowedTimeMinutes;
                switch (exam.LevelId)
                {
                    case 1:
                        allowedTimeMinutes = 30;
                        break;
                    case 2:
                        allowedTimeMinutes = 45;
                        break;
                    case 3:
                        allowedTimeMinutes = 60;
                        break;
                    default:
                        allowedTimeMinutes = 30;
                        break;
                }


                TimeSpan elapsedTime = DateTime.UtcNow - exam.StartedAt;
                bool isTimeUp = elapsedTime.TotalMinutes > allowedTimeMinutes;

                int score = 0;

                // Process answers only if time is not fully expired
                if (!isTimeUp && dto.Answers != null)
                {
                    foreach (var answer in dto.Answers)
                    {
                        // Skip if no option was selected
                        if (answer.SelectedOptionId <= 0)
                            continue;

                        var option = _optionRepository.GetById(answer.SelectedOptionId);

                        // Skip if the option is invalid
                        if (option == null)
                            continue;

                        bool isCorrect = option.IsCorrect;
                        if (isCorrect) score++;

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

                var tech = _techRepository.GetById(exam.TechId);

                // Finalize exam
                bool isPassed = !isTimeUp && score >= exam.Level.PassMarks;

                exam.Score = score;
                exam.Status = isPassed;
                exam.CompletedAt = isTimeUp
                    ? exam.StartedAt.AddMinutes(allowedTimeMinutes)
                    : DateTime.UtcNow;

                _examRepository.Update(exam);
                _resultRepository.SaveChanges();
                _examRepository.SaveChanges();

                // Response
                return new ApiResponse<SubmitExamResponseDTO>
                {
                    Success = !isTimeUp,
                    Message = isTimeUp? "Time is up! Exam automatically submitted as failed." : "Exam submitted successfully",
                    Data = new SubmitExamResponseDTO
                    {
                        UserName = exam.User.FullName,
                        Technology = tech?.Name ?? "Unknown",
                        Level = exam.Level.LevelName,
                        Score = score,
                        TimeTakenMinutes = (int)Math.Min(elapsedTime.TotalMinutes, allowedTimeMinutes),
                        Result = isPassed ? "Pass" : "Fail"
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubmitExamResponseDTO>
                {
                    Success = false,
                    Message = "Error while submitting exam: " + ex.Message
                };
            }
        }

        // Report Service
        public ApiResponse<ExamResultDTO> GetExamResult(int examId, int userId)
        {
            try
            {
                var exam = _examRepository.GetById(examId);
                if (exam == null)
                    return new ApiResponse<ExamResultDTO>
                    {
                        Success = false,
                        Message = "Exam not found"
                    };

                if (exam.UserId != userId)
                    return new ApiResponse<ExamResultDTO>
                    {
                        Success = false,
                        Message = "Unauthorized access"
                    };

                if (exam.CompletedAt == null)
                    return new ApiResponse<ExamResultDTO>
                    {
                        Success = false,
                        Message = "Exam not yet completed"
                    };

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
                        SelectedOption = r.Option?.OptionText ?? "Not answered",
                        CorrectOption = r.Question.Options.FirstOrDefault(o => o.IsCorrect)?.OptionText ?? "N/A",
                        IsCorrect = r.IsCorrect
                    }).ToList()
                };

                return new ApiResponse<ExamResultDTO>
                {
                    Success = true,
                    Message = "Exam result fetched successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ExamResultDTO>
                {
                    Success = false,
                    Message = "Error while fetching exam result: " + ex.Message
                };
            }
        }


    }
}