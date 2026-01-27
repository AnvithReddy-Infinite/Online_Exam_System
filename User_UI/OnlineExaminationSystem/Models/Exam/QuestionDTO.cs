using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models.Exam
{
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<OptionDTO> Options { get; set; }
    }
}