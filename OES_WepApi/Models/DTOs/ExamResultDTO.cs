using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models.DTOs
{
    public class ExamResultDTO
    {
        public int ExamId { get; set; }
        public string UserName { get; set; }
        public string Technology { get; set; }
        public string Level { get; set; }
        public int Score { get; set; }
        public int PassMarks { get; set; }
        public string Result { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<QuestionResultDTO> Questions { get; set; }
    }
}