using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models.DTOs
{
    public class StartExamDTO
    {
        public int UserId { get; set; }   // current user
        public int TechId { get; set; }   // technology selected
        public int LevelId { get; set; }  // level selected
    }
}