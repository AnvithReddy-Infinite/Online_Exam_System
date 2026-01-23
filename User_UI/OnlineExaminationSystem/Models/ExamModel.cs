using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class ExamModel
    {
        [Key]
        public int ExamId { get; set; }
        [Required(ErrorMessage = "Exam Name is required")]
        [StringLength(100)]
        public string ExamName { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(50)]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Level is required")]
        [StringLength(20)]
        public string Level { get; set; } // e.g., Easy, Medium, Hard
        [Required(ErrorMessage = "Number of Questions is required")]
        [Range(1, 100, ErrorMessage = "Questions count must be between 1 and 100")]
        public int QuestionsCount { get; set; }
        // Optional: exam duration in minutes
        [Range(1, 180, ErrorMessage = "Duration must be between 1 and 180 minutes")]
        public int Duration { get; set; }
    }
}
