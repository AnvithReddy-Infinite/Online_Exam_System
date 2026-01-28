using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Models
{
    public class AdminQuestionViewModel
    {
        // Dropdowns
        public int TechId { get; set; }
        public int LevelId { get; set; }

        public List<SelectListItem> Technologies { get; set; }
        public List<SelectListItem> Levels { get; set; }

        // File upload
        public HttpPostedFileBase File { get; set; }
    }
}

