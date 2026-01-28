using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem.Models
{
    public class AdminExamViewModel
    {
        [Required(ErrorMessage = "Exam name is required")]
        public string ExamName { get; set; }

        [Required(ErrorMessage = "Technology is required")]
        public string Technology { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Number of questions is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of questions must be greater than 0")]
        public int? NumberOfQuestions { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public int? Duration { get; set; }
    }
}
