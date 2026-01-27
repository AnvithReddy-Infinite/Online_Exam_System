using FinalProject.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES_WebAPI.Models.dtos
{
    public class UserExamReportDTO
    {
        public int ExamId { get; set; }
        public string UserName { get; set; }          // Name of the user
        public string ExamName { get; set; }
        public string Technology { get; set; }       // e.g., "C#"
        public string Level { get; set; }            // e.g., "Intermediate"
        public int Score { get; set; }               // Score obtained
        public int PassMarks { get; set; }           // Passing marks for the level
        public string Result { get; set; }           // Pass/Fail
        public DateTime StartedAt { get; set; }      // Exam start time
        public DateTime? CompletedAt { get; set; }   // Exam completion time
        public List<QuestionResultDTO> Questions { get; set; } = new List<QuestionResultDTO>();
        public DateTime DateTaken { get; set; }
        public int TotalMarks { get; set; }
    }
}