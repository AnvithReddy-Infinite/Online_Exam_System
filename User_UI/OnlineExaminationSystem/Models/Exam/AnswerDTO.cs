using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.Exam
{
    public class AnswerDTO
    {
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
}