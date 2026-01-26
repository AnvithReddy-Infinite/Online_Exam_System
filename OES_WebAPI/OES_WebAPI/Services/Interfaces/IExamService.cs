using FinalProject.Common;
using FinalProject.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services.Interfaces
{
    public interface IExamService
    {
        ApiResponse<StartExamResponseDTO> StartExam(StartExamDTO dto);
        ApiResponse<List<QuestionDTO>> GetQuestions(int examId, int userId);
        ApiResponse<SubmitExamResponseDTO> SubmitExam(SubmitExamDTO dto);
        ApiResponse<ExamResultDTO> GetExamResult(int examId, int userId);
    }
}
