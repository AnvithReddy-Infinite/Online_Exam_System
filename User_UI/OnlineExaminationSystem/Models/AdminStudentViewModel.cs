using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Models
{
    public class AdminStudentViewModel
    {
        // Search filters
        public int? TechId { get; set; }
        public int? LevelId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int? MinMarks { get; set; }

        // Dropdowns
        public List<SelectListItem> Technologies { get; set; }
        public List<SelectListItem> Levels { get; set; }

        // Results
        public List<StudentResultViewModel> Students { get; set; }
    }

    public class StudentResultViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int TechId { get; set; }
        public int LevelId { get; set; }
        public int Score { get; set; }
    }
}
