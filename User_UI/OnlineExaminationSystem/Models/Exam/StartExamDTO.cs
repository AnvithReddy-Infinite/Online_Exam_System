using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Models.Exam
{
    public class StartExamDTO
    {
        public int UserId { get; set; }
        public int TechId { get; set; }
        public int LevelId { get; set; }

    }
}