using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.Exam
{
    public class QuestionListResponseDTO
    {
        public int ExamId { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}