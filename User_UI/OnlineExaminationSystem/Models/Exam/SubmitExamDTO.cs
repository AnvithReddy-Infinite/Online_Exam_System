using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.Exam
{
    public class SubmitExamDTO
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}