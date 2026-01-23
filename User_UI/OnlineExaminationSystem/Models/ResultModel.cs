using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class ResultModel
    {
        [Key]
        public int ResultId { get; set; }
        [Required]
        public int UserId { get; set; } // Reference to User
        [Required]
        public int ExamId { get; set; } // Reference to Exam
        [Required]
        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
        public int MarksObtained { get; set; }
        [Required]
        public DateTime ExamDate { get; set; }
        // Optional: pass/fail status
        public string Status
        {
            get
            {
                return MarksObtained >= 40 ? "Pass" : "Fail"; // example: pass >=40
            }
        }
    }
}