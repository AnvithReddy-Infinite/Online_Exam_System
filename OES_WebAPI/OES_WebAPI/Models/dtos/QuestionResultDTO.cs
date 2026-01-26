using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models.DTOs
{
    public class QuestionResultDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectOption { get; set; }
        public bool IsCorrect { get; set; }
    }
}