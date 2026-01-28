using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem.Models
{
    public class AdminQuestionViewModel
    {
        [Required(ErrorMessage = "Technology is required")]
        public string Technology { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Question is required")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Option D is required")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = "Correct answer is required")]
        public string CorrectAnswer { get; set; }

        public HttpPostedFileBase QuestionFile { get; set; }

    }
}
